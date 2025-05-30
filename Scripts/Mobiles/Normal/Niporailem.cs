using System;
using System.Collections.Generic;
using System.Linq;

using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("the corpse of niporailem")]
    public class Niporailem : BaseSABoss
    {
        public override Type[] UniqueSAList { get { return new Type[] { typeof(HelmOfVillainousEpiphany), typeof(GorgetOfVillainousEpiphany), typeof(BreastplateOfVillainousEpiphany),
                                                                        typeof(ArmsOfVillainousEpiphany), typeof(GauntletsOfVillainousEpiphany), typeof(LegsOfVillainousEpiphany),
                                                                        typeof(KiltOfVillainousEpiphany), typeof(EarringsOfVillainousEpiphany), typeof(GargishBreastplateOfVillainousEpiphany),
                                                                        typeof(GargishArmsOfVillainousEpiphany), typeof(NecklaceOfVillainousEpiphany), typeof(GargishLegsOfVillainousEpiphany),
                                                                        typeof(HelmOfVirtuousEpiphany), typeof(GorgetOfVirtuousEpiphany), typeof(BreastplateOfVirtuousEpiphany),
                                                                        typeof(ArmsOfVirtuousEpiphany), typeof(GauntletsOfVirtuousEpiphany), typeof(LegsOfVirtuousEpiphany),
                                                                        typeof(KiltOfVirtuousEpiphany), typeof(EarringsOfVirtuousEpiphany), typeof(GargishBreastplateOfVirtuousEpiphany),
                                                                        typeof(GargishArmsOfVirtuousEpiphany), typeof(NecklaceOfVirtuousEpiphany), typeof(GargishLegsOfVirtuousEpiphany)}; } }
        
        public override Type[] SharedSAList { get { return new Type[] { typeof(BladeOfBattle), typeof(DemonBridleRing), typeof(GiantSteps), typeof(SwordOfShatteredHopes) }; } }

        [Constructable]
        public Niporailem()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Niporailem";
            Title = "the Thief";

            Body = 722;

            SetStr(1000);
            SetDex(1200);
            SetInt(1200);

            SetHits(10000, 10500);

            SetDamage(15, 27);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 34, 46);
            SetResistance(ResistanceType.Fire, 0);
            SetResistance(ResistanceType.Cold, 31, 49);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 31, 49);

            SetSkill(SkillName.Wrestling, 68.8, 85.0);
            SetSkill(SkillName.Tactics, 56.1, 90.0);
            SetSkill(SkillName.MagicResist, 87.7, 93.5);

            SetSkill(SkillName.EvalInt, 90.0, 100.0);
            SetSkill(SkillName.Meditation, 20.0, 30.0);
            SetSkill(SkillName.Necromancy, 120.0);
            SetSkill(SkillName.SpiritSpeak, 120.0);
            SetSkill(SkillName.Focus, 30.0, 40.0);

            PackNecroReg(12, 24); /// Stratics didn't specify

            Fame = 15000;
            Karma = -15000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 6);
            AddLoot(LootPack.Gems, 6);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new NiporailemsShroud());
            }			
        }

        public override int Meat { get { return 1; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override int GetIdleSound() { return 1609; }
        public override int GetAngerSound() { return 1606; }
        public override int GetHurtSound() { return 1608; }
        public override int GetDeathSound() { return 1607; }

        public Niporailem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.WriteMobileList(Helpers, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    Helpers = reader.ReadStrongMobileList<BaseCreature>();
                    break;
            }
        }

        public List<BaseCreature> Helpers { get; set; } = new List<BaseCreature>();
        private DateTime m_NextTreasure;
        private int m_Thrown;
        private DateTime m_NextSpawn;

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (m_NextSpawn > DateTime.UtcNow || Helpers.Where(bc => bc.Deleted).Count() > 10)
                return;

            if (this.Hits > (this.HitsMax / 4))
            {
                if (0.25 >= Utility.RandomDouble())
                {
                    SpawnSpectralArmour(attacker);
                }
            }
            else if (0.10 >= Utility.RandomDouble())
            {
                SpawnSpectralArmour(attacker);
            }
        }

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 20) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= m_NextTreasure)
            {
                ThrowTreasure(combatant);

                m_Thrown++;

                if (0.75 >= Utility.RandomDouble() && (m_Thrown % 2) == 1) // 75% chance to toss a second one
                    m_NextTreasure = DateTime.UtcNow + TimeSpan.FromSeconds(3.0);
                else
                    m_NextTreasure = DateTime.UtcNow + TimeSpan.FromSeconds(5.0 + (10.0 * Utility.RandomDouble())); // 5-15 seconds
            }
        }

        public void SpawnSpectralArmour(Mobile m)
        {
            Map map = this.Map;

            if (map == null)
                return;

            SpectralArmour spawned = new SpectralArmour();

            spawned.Team = this.Team;
            spawned.SummonMaster = this;

            bool validLocation = false;
            Point3D loc = this.Location;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);

                if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                    loc = new Point3D(x, y, Z);
                else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    loc = new Point3D(x, y, z);
            }

            spawned.MoveToWorld(loc, map);
            spawned.Combatant = m; spawned.SummonMaster = this;

            m_NextSpawn = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));

            Helpers.Add(spawned);
        }

        public void DeleteSpectralArmour(Mobile target)
        {
            foreach (var m in Helpers.Where(bc => bc != null && !bc.Deleted))
            {
                m.Delete();
            }

            ColUtility.Free(Helpers);
        }

        public override void OnDelete()
        {
            DeleteSpectralArmour(this);

            base.OnDelete();
        }

        private void ThrowTreasure(Mobile m)
        {
            DoHarmful(m);

            this.MovingParticles(m, 0xEEF, 9, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                var treasure = new NiporailemsTreasure(this);

                m.PlaySound(0x033);
                m.AddToBackpack(treasure);
                m.SendLocalizedMessage(1112111); // To steal my gold? To give it freely!
            });
        }
    }
}
