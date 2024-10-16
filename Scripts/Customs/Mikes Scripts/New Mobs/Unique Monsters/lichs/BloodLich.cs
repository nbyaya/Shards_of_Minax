using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blood lich's corpse")]
    public class BloodLich : BaseCreature
    {
        private DateTime m_NextBloodSacrifice;
        private DateTime m_NextBloodCurse;
        private DateTime m_NextCrimsonRitual;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BloodLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blood lich";
            Body = 24;
            Hue = 2148; // Dark red hue
			BaseSoundID = 0x3E9;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public BloodLich(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextBloodSacrifice = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBloodCurse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCrimsonRitual = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBloodSacrifice)
                {
                    BloodSacrifice();
                }

                if (DateTime.UtcNow >= m_NextBloodCurse)
                {
                    BloodCurse();
                }

                if (DateTime.UtcNow >= m_NextCrimsonRitual)
                {
                    CrimsonRitual();
                }
            }
        }

        private void BloodSacrifice()
        {
            if (Hits > 50)
            {
                int sacrificeAmount = 30;
                Hits -= sacrificeAmount;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Blood Sacrifice! *");
                PlaySound(0x1F1);
                FixedEffect(0x376A, 10, 16);

                // Cast a spell or heal itself (use a healing spell or similar effect)
                Heal(sacrificeAmount); // Adjust healing effect if needed

                m_NextBloodSacrifice = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void BloodCurse()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Blood Curse! *");
                PlaySound(0x1F1);
                FixedEffect(0x376A, 10, 16);

                // Apply blood curse effect (damage over time)
                target.SendMessage("You are cursed and start bleeding!");
                Timer.DelayCall(TimeSpan.FromSeconds(5), () => ApplyBleedDamage(target));

                m_NextBloodCurse = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            }
        }

        private void ApplyBleedDamage(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                int bleedDamage = Utility.RandomMinMax(10, 20);
                target.Damage(bleedDamage, this);
                target.SendMessage("You continue to bleed from the curse!");
            }
        }

        private void CrimsonRitual()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Crimson Ritual! *");
            PlaySound(0x1F1);
            FixedEffect(0x376A, 10, 16);

            // Temporarily increase power and summon minions
            SetStr(Str + 50);
            SetDex(Dex + 30);
            SetInt(Int + 20);

            Point3D loc = GetSpawnPosition(5);
            if (loc != Point3D.Zero)
            {
                for (int i = 0; i < 3; i++)
                {
                    BloodMinion minion = new BloodMinion();
                    minion.MoveToWorld(loc, Map);
                }
            }

            m_NextCrimsonRitual = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }

    public class BloodMinion : BaseCreature
    {
        public BloodMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blood minion";
            Body = 0x19; // Adjust body as needed
            Hue = 1266; // Dark red hue

            SetStr(100, 150);
            SetDex(100, 125);
            SetInt(50, 75);

            SetHits(100, 120);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Poison, 70, 90);

            SetSkill(SkillName.Tactics, 70.0);
            SetSkill(SkillName.Wrestling, 70.0);

            VirtualArmor = 40;
        }

        public BloodMinion(Serial serial)
            : base(serial)
        {
        }

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
