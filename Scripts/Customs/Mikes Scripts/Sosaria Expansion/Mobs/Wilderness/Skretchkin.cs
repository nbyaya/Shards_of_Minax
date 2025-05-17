using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;               // for Poison
using Server.SkillHandlers;        // if DefaultSkillMod resides here; otherwise Server namespace

namespace Server.Mobiles
{
    [CorpseName("a skretchkin corpse")]
    public class Skretchkin : BaseCreature
    {
        #region Constructor / Base Stats
        [Constructable]
        public Skretchkin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Skretchkin";
            Body = 307;
            BaseSoundID = 422;
            Hue = 0x8A;

            SetStr(250, 300);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(350, 450);
            SetStam(150, 200);
            SetMana(100, 150);

            SetDamage(18, 25);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 80.1, 95.0);
            SetSkill(SkillName.Tactics,      90.1, 100.0);
            SetSkill(SkillName.Wrestling,    95.1, 105.0);
            SetSkill(SkillName.EvalInt,      80.0, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 45;

            SetWeaponAbility(WeaponAbility.Dismount);
        }
        #endregion

        #region Ability Fields
        private enum SkretchkinAbilityState { Idle, PreparingWindAoe }
        private SkretchkinAbilityState _CurrentAbilityState = SkretchkinAbilityState.Idle;
        private DateTime _NextAbilityTime;
        private readonly TimeSpan WindupDuration       = TimeSpan.FromSeconds(2.5);
        private const int    WindAoeRadius            = 4;
        private const int    WindAoeMinDamage         = 30;
        private const int    WindAoeMaxDamage         = 50;
        private const double WindAoePoisonChance      = 0.75;
        private static readonly Poison WindAoePoisonLevel = Poison.Deadly;
        private const int    WindAoeSoundID           = 0x574;
        private const int    WindAoeParticleEffect    = 0x3709;
        private const int    WindAoeParticleHue       = 0x3F;
        #endregion

        #region Combat Override (handles cooldowns + wind‑up)
        public override void OnActionCombat()
        {
            // If we're still cooling down, just fight normally
            if (DateTime.UtcNow < _NextAbilityTime)
            {
                base.OnActionCombat();
                return;
            }

            switch (_CurrentAbilityState)
            {
                case SkretchkinAbilityState.Idle:
                    var tgt = Combatant;
                    if (tgt is Mobile m && !m.Deleted && m.Map == Map && InRange(m, WindAoeRadius + 2) && CanBeHarmful(m) && InLOS(m))
                    {
                        if (Utility.RandomDouble() < 0.3)
                            StartCorrosiveGustWindup();
                        else
                            base.OnActionCombat();
                    }
                    else
                    {
                        base.OnActionCombat();
                    }
                    break;

                case SkretchkinAbilityState.PreparingWindAoe:
                    // still winding up—do nothing until the timer fires
                    break;
            }
        }

        private void StartCorrosiveGustWindup()
        {
            _CurrentAbilityState = SkretchkinAbilityState.PreparingWindAoe;

            Animate(10, 5, 1, true, false, 0);
            PublicOverheadMessage(MessageType.Regular, Hue, false, "Skretchkin shudders and prepares a gust!");

            Timer.DelayCall(WindupDuration, PerformCorrosiveGust);
        }

        private void PerformCorrosiveGust()
        {
            if (_CurrentAbilityState != SkretchkinAbilityState.PreparingWindAoe)
            {
                _CurrentAbilityState = SkretchkinAbilityState.Idle;
                return;
            }

            Effects.PlaySound(Location, Map, WindAoeSoundID);



            foreach (var mob in AcquireTargetsInRadius(Location, WindAoeRadius))
            {
                if (mob == this || mob.Deleted || mob.Map != Map || !CanBeHarmful(mob))
                    continue;

                int damage = Utility.RandomMinMax(WindAoeMinDamage, WindAoeMaxDamage);
                AOS.Damage(mob, this, damage, 0, 0, 0, 50, 50);

                if (Utility.RandomDouble() <= WindAoePoisonChance)
                {
                    mob.ApplyPoison(this, WindAoePoisonLevel);
                    mob.SendMessage(Hue, "You feel a burning sickness from the gust!");
                }
            }

            // reset & stagger next use
            _CurrentAbilityState = SkretchkinAbilityState.Idle;
            _NextAbilityTime = DateTime.UtcNow + WindupDuration + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
        }

        private List<Mobile> AcquireTargetsInRadius(Point3D p, int radius)
        {
            var list = new List<Mobile>();
            var eable = Map.GetMobilesInRange(p, radius);

            foreach (Mobile m in eable)
            {
                if (m != this && !m.Deleted && m.Map == Map && CanBeHarmful(m))
                    list.Add(m);
            }
            eable.Free();
            return list;
        }
        #endregion

        #region Melee Hit Effects
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender == null || defender.Deleted || defender.Map != Map)
                return;

            // 25% chance: -20 Tactics & Wrestling for 5 seconds
            if (Utility.RandomDouble() < 0.25)
            {
                TimeSpan dur = TimeSpan.FromSeconds(5.0);
                int deb = 20;

                var modT = new DefaultSkillMod(SkillName.Tactics,    false, -deb);
                var modW = new DefaultSkillMod(SkillName.Wrestling,  false, -deb);

                defender.AddSkillMod(modT);
                defender.AddSkillMod(modW);

				Timer.DelayCall(dur, () =>
				{
					modT.Remove();
					modW.Remove();
				});


                defender.SendMessage("Skretchkin's touch weakens your combat skills!");
            }

            // 35% chance: light poison
            if (Utility.RandomDouble() < 0.35)
            {
                defender.ApplyPoison(this, Poison.Regular);
                defender.SendMessage("You feel a mild sickness from Skretchkin's touch!");
            }
        }
        #endregion

        #region Loot, Rummage & Serialization
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 4;
        public override int Meat             => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Potions);
            if (Utility.RandomDouble() < 0.01)
            {
                // PackItem(new SkretchkinAcidGland());
            }
        }

        public Skretchkin(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
            writer.Write((int)_CurrentAbilityState);
            writer.Write(_NextAbilityTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                _CurrentAbilityState = (SkretchkinAbilityState)reader.ReadInt();
                _NextAbilityTime      = reader.ReadDateTime();
            }

            // reset any partial wind‑up
            _CurrentAbilityState = SkretchkinAbilityState.Idle;
        }
        #endregion
    }
}
