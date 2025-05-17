using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // for ResistanceMod, etc.

namespace Server.Mobiles
{
    [CorpseName("a pyrrfelis corpse")]
    public class Pyrrfelis : BaseCreature
    {
        private DateTime m_NextInfernoBlast;
        private DateTime m_InfernoBlastWindup;
        private bool m_PerformingInfernoBlast;

        [Constructable]
        public Pyrrfelis()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a pyrrfelis";
            Body = 0xC9; // Hell Cat body
            BaseSoundID = 0x69; // Hell Cat sounds
            Hue = 0x489; // A unique deep red/orange hue

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(700, 850);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire,     70);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     90,100);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   30, 40);

            SetSkill(SkillName.MagicResist, 80.1, 95.0);
            SetSkill(SkillName.Tactics,     85.1,100.0);
            SetSkill(SkillName.Wrestling,   90.1,105.0);
            SetSkill(SkillName.Inscribe,    80.0, 90.0);
            SetSkill(SkillName.Magery,      80.0, 90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 40;

            Tamable      = false;
            ControlSlots = 5;
            MinTameSkill = 120.0;

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public Pyrrfelis(Serial serial)
            : base(serial)
        {
        }

        public override int Hides           => 20;
        public override HideType HideType  => HideType.Barbed;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Feline;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls);
            AddLoot(LootPack.Potions);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new LavaTile());
        }

        // --- Inferno Blast (AOE) ---
        public void InfernoBlast(Mobile target)
        {
            if (target == null || target.Deleted || m_PerformingInfernoBlast)
                return;

            m_PerformingInfernoBlast  = true;
            m_InfernoBlastWindup      = DateTime.UtcNow + TimeSpan.FromSeconds(3.0);

            PrivateOverheadMessage(MessageType.Regular, Hue, false, "*starts charging an inferno blast!*", NetState);
            PlaySound(0x5AA);

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
            {
                if (Deleted || !m_PerformingInfernoBlast) return;
                m_PerformingInfernoBlast = false;
                m_NextInfernoBlast       = DateTime.UtcNow + TimeSpan.FromSeconds(15.0 + (10.0 * Utility.RandomDouble()));

                Map map = Map;
                if (map == null) return;

                PlaySound(0x658);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 10, 30, 0x489, 0, 2023, 0
                );
                Effects.PlaySound(Location, map, 0x208);

                var eable = map.GetMobilesInRange(Location, 6);
                var targets = new List<Mobile>();

                foreach (Mobile m in eable)
                    if (m != this && m is PlayerMobile && CanBeHarmful(m))
                        targets.Add(m);

                eable.Free();

                foreach (var m in targets)
                {
                    DoHarmful(m);

                    int rawDamage = Utility.RandomMinMax(40, 60)
                                  + (int)(Skills[SkillName.Magery].Value / 5);
                    int fireRes   = m.FireResistance;
                    int dmg       = (int)(rawDamage * (1.0 - fireRes / 100.0));
                    dmg = Math.Max(1, dmg);

                    AOS.Damage(m, this, dmg, 0, 100, 0, 0, 0);
                    m.SendAsciiMessage("You are engulfed in flames!");

                    var mod = new ResistanceMod(ResistanceType.Fire, -15);
                    m.AddResistanceMod(mod);

                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                    {
                        m.RemoveResistanceMod(mod);
                        if (m.NetState != null)
                            m.SendAsciiMessage("Your fire resistance returns to normal.");
                    });
                }
            });
        }

        // --- Fiery Pounce ---
        public void FieryPounce(Mobile target)
        {
            if (target == null || target.Deleted || !InRange(target, 10) || !InLOS(target))
                return;

            PrivateOverheadMessage(MessageType.Regular, Hue, false, "*prepares to pounce!*", NetState);
            PlaySound(0x5DF);

            Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
            {
                if (Deleted) return;

                Map map = Map;
                if (map == null) return;

                Point3D pLoc = target.Location;
                int tries = 0;
                bool found = false;

                while (!found && tries < 10)
                {
                    var test = new Point3D(
                        pLoc.X + Utility.RandomMinMax(-1,1),
                        pLoc.Y + Utility.RandomMinMax(-1,1),
                        pLoc.Z
                    );

                    if (map.CanSpawnMobile(test))
                    {
                        pLoc  = test;
                        found = true;
                    }
                    tries++;
                }

                Effects.SendMovingParticles(
                    this, target, 0x36FE, 10, 0, false, false,
                    0x489, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
                );

                MoveToWorld(pLoc, map);
                PlaySound(0x5E1);

                DoHarmful(target);
                int pdmg = Utility.RandomMinMax(20,30);
                AOS.Damage(target, this, pdmg, 70, 30, 0, 0, 0);

                if (Utility.RandomDouble() < 0.3)
                {
                    target.ApplyPoison(this, Poison.Lesser);
                    target.SendAsciiMessage("You are burned by the Pyrrfelis' fiery pounce!");
                }
            });
        }

        // --- Heat Aura ---
        public void ApplyHeatAura()
        {
            if (Deleted || Map == null) return;

            var eable = Map.GetMobilesInRange(Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && m is PlayerMobile && CanBeHarmful(m)
                 && Utility.RandomDouble() < 0.05)
                {
                    m.ApplyPoison(this, Poison.Lesser);
                    m.SendAsciiMessage("The heat from the Pyrrfelis singes you!");
                }
            }
            eable.Free();
        }

        public override void OnThink()
        {
            base.OnThink();
            ApplyHeatAura();

			Mobile m = Combatant as Mobile;
			if (m != null && !m_PerformingInfernoBlast
			 && DateTime.UtcNow >= m_NextInfernoBlast
			 && InRange(m, 8))
			{
				InfernoBlast(m);
			}

			if (m != null && !m_PerformingInfernoBlast
			 && Utility.RandomDouble() < 0.005)
			{
				FieryPounce(m);
			}

        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.2)
            {
                defender.ApplyPoison(this, Poison.Lesser);
                defender.SendAsciiMessage("You are burned by the Pyrrfelis' fiery claws!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(m_NextInfernoBlast);
            writer.Write(m_InfernoBlastWindup);
            writer.Write(m_PerformingInfernoBlast);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
            {
                m_NextInfernoBlast   = reader.ReadDateTime();
                m_InfernoBlastWindup = reader.ReadDateTime();
                m_PerformingInfernoBlast = reader.ReadBool();
            }
        }
    }
}
