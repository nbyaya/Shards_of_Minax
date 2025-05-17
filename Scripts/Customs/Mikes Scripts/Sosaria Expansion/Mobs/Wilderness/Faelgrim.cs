using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Engines.CannedEvil;
using Server.Targeting;
using Server.Misc;             // Utility
using Server.Mobiles;          // for TimedSkillMod
using Server.Mobiles;          // for ResistanceMod

namespace Server.Mobiles
{
    [CorpseName("a faelgrim corpse")]
    public class Faelgrim : BaseCreature
    {
        // --- Windup AOE attack state ---
        private DateTime _NextWindupAttack;
        private readonly TimeSpan _WindupDuration = TimeSpan.FromSeconds(3.0);
        private Point3D _WindupTargetLocation;

        // --- Mystical Barrier state ---
        private DateTime _NextMysticalBarrier;
        private bool _BarrierActive;
        private ResistanceMod _physMod, _energyMod;

        [Constructable]
        public Faelgrim()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Faelgrim";
            Body = 271;
            BaseSoundID = 0x586;
            Hue = 0x481;

            SetStr(500, 600);
            SetDex(300, 350);
            SetInt(250, 300);

            SetHits(1200, 1500);
            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   60, 70);

            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics,     100.0, 110.0);
            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.Magery,      80.0,  90.0);
            SetSkill(SkillName.EvalInt,     80.0,  90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 40;

            SetSkill(SkillName.Musicianship, 80.0);
            SetSkill(SkillName.Discordance,  80.0);
            SetSkill(SkillName.Provocation,  80.0);
            SetSkill(SkillName.Peacemaking,  80.0);
        }
		
		public Faelgrim(Serial serial) : base(serial) { }

        public override TribeType Tribe => TribeType.Fey;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override bool CanDiscord => true;
        public override bool CanPeace => false;
        public override bool CanProvoke => true;
        public override int Meat => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls);
            AddLoot(LootPack.Gems);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new ArtifactSatyrsCloak());
        }

        // --- Unique Ability 1: Soul Drain ---
        public void SoulDrain(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            if (!InRange(target.Location, 5))
                return;

            Effects.SendMovingEffect(this, target, 0x367A, 5, 0, false, false, 0, 0);
            PlaySound(0x1E0);

            int manaDrain = Utility.RandomMinMax(20, 40);
            int stamDrain = Utility.RandomMinMax(20, 40);

            target.Mana -= manaDrain;
            target.Stam -= stamDrain;
            target.SendLocalizedMessage(1060091); // You feel drained!

            // 25% chance to apply a curse
            if (Utility.RandomDouble() < 0.25)
            {
                Effects.SendMovingEffect(this, target, 0x367A, 5, 0, false, false, 0, 0);

                int curseType = Utility.Random(3);
                TimeSpan duration = TimeSpan.FromSeconds(15);

                switch (curseType)
                {
                    case 0:
                        // Tactics skill curse
                        target.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, -10, duration));
                        target.SendMessage("You feel less able to fight!");  // :contentReference[oaicite:2]{index=2}
                        break;
                    case 1:
                        // Wrestling skill curse
                        target.AddSkillMod(new TimedSkillMod(SkillName.Wrestling, true, -10, duration));
                        target.SendMessage("Your grip weakens!");
                        break;
                    default:
                        // Dexterity stat curse
                        SpellHelper.AddStatCurse(this, target, StatType.Dex, -5, duration);
                        target.SendMessage("You feel sluggish!");
                        break;
                }
            }
        }

        // --- Unique Ability 2: Mystical Barrier ---
        public void MysticalBarrier()
        {
            if (_BarrierActive || DateTime.UtcNow < _NextMysticalBarrier)
                return;

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x375A, 10, 15, 5012
            );
            PlaySound(0x1EA);

            int physBonus   = 10;
            int energyBonus = 15;

            _physMod   = new ResistanceMod(ResistanceType.Physical, physBonus);
            _energyMod = new ResistanceMod(ResistanceType.Energy,   energyBonus);

            AddResistanceMod(_physMod);
            AddResistanceMod(_energyMod);

            _BarrierActive = true;

            // Remove buffs after 20s
            Timer.DelayCall(TimeSpan.FromSeconds(20.0), delegate
            {
                RemoveResistanceMod(_physMod);
                RemoveResistanceMod(_energyMod);
                _BarrierActive = false;
            });

            _NextMysticalBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(30.0);
        }

        // --- Advanced AOE Attack: Ethereal Burst ---
        public void EtherealBurst()
        {
            if (Combatant == null || !(Combatant is Mobile) || DateTime.UtcNow < _NextWindupAttack)
                return;

            Mobile targ = (Mobile)Combatant;
            _WindupTargetLocation = targ.Location;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*begins channeling ethereal energy!*");
            FixedParticles(0x37C4, 1, 24, 9904, 2583, 7, EffectLayer.Waist);
            PlaySound(0x19C);

            _NextWindupAttack = DateTime.UtcNow + _WindupDuration;
            Timer.DelayCall(_WindupDuration, new TimerStateCallback(FinishEtherealBurst), _WindupTargetLocation);
        }

        public void FinishEtherealBurst(object state)
        {
            Point3D loc = (Point3D)state;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*unleashes an ethereal burst!*");
            Effects.PlaySound(loc, Map, 0x20F);
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, 2023
            );

            var list = Map.GetMobilesInRange(loc, 6);
            foreach (Mobile m in list)
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                DoHarmful(m);

                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(m, this, damage, 0,0,0,0,100);

                if (Utility.RandomDouble() < 0.3)
                {
                    TimeSpan dur = TimeSpan.FromSeconds(10);
                    if (Utility.RandomBool())
                    {
                        m.AddSkillMod(new TimedSkillMod(SkillName.MagicResist, true, -5, dur));
                        m.SendMessage("Your magical defenses feel weaker!");
                    }
                    else
                    {
                        SpellHelper.AddStatCurse(this, m, StatType.Int, -5, dur);
                        m.SendMessage("Your mind feels clouded!");
                    }
                }
            }
            list.Free();

            _NextWindupAttack = DateTime.UtcNow + TimeSpan.FromSeconds(15.0 + (10.0 * Utility.RandomDouble()));
        }

        public override void OnThink()
        {
            base.OnThink();

            var combatant = Combatant as Mobile;

            if (combatant != null && InRange(combatant, 5) && Utility.RandomDouble() < 0.05)
                SoulDrain(combatant);

            if (!_BarrierActive && Utility.RandomDouble() < 0.02)
                MysticalBarrier();

            if (DateTime.UtcNow >= _NextWindupAttack && combatant != null && InRange(combatant, 10))
                EtherealBurst();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.15 && defender is PlayerMobile)
            {
                defender.SendMessage("You are knocked back by the Faelgrim's strike!");
                defender.BoltEffect(0);
                defender.PlaySound(0x2F3);

                // defender.KnockbackFrom(Location, 2);  <-- removed, no longer exists
                // You can add your own movement here if desired.
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        // Example unique artifact
        public class ArtifactSatyrsCloak : BaseCloak
        {
            [Constructable]
            public ArtifactSatyrsCloak() : base(0x1515)
            {
                Name = "Satyr's Cloak";
                Hue = 0x489;

                Attributes.BonusInt      = 5;
                Attributes.SpellDamage   = 10;
                Attributes.LowerManaCost = 5;
                Resistances.Energy       = 10;
            }

            public ArtifactSatyrsCloak(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
