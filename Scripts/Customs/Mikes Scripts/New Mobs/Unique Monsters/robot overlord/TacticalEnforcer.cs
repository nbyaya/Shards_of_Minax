using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a tactical enforcer corpse")]
    public class TacticalEnforcer : BaseCreature
    {
        private DateTime m_NextPrecisionStrike;
        private DateTime m_NextTacticalRetreat;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TacticalEnforcer()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Tactical Enforcer";
            Body = 0x2F4; // ExodusOverseer body
            BaseSoundID = 0x2F8;
            Hue = 2283; // Unique metallic hue

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

            PackItem(new PowerCrystal());
            PackItem(new ArcaneGem());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public TacticalEnforcer(Serial serial)
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
        public override int GetIdleSound()
        {
            return 0xFD;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x23B;
        }

        public override int GetHurtSound()
        {
            return 0x140;
        }
        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                // Initialize ability timings if not done already
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPrecisionStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextTacticalRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                double healthPercentage = (double)Hits / HitsMax * 100;

                if (DateTime.UtcNow >= m_NextPrecisionStrike)
                {
                    DoPrecisionStrike();
                }

                if (DateTime.UtcNow >= m_NextTacticalRetreat && healthPercentage < 50)
                {
                    DoTacticalRetreat();
                }
            }
        }

        private void DoPrecisionStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Precision Strike *");
            PlaySound(0x5BF);

            FixedEffect(0x376A, 10, 15); // Targeting reticle effect

            if (Combatant is Mobile target && target.Alive)
            {
                target.FixedEffect(0x37B9, 10, 5); // Hit effect
                int damage = Utility.RandomMinMax(50, 65);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                target.SendLocalizedMessage(1032011); // You have been precision struck!
            }

            m_NextPrecisionStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Fixed cooldown after ability use
        }

        private void DoTacticalRetreat()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Tactical Retreat *");
            PlaySound(0x210);

            Point3D newLocation = GetRetreatLocation();

            if (newLocation != Point3D.Zero)
            {
                FixedEffect(0x376A, 10, 20); // Teleport effect at current location
                MoveToWorld(newLocation, Map);
                FixedEffect(0x376A, 10, 20); // Teleport effect at new location
                m_NextTacticalRetreat = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown after ability use
            }
        }

        private Point3D GetRetreatLocation()
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(5, 10) * (Utility.RandomBool() ? 1 : -1);
                int y = Y + Utility.RandomMinMax(5, 10) * (Utility.RandomBool() ? 1 : -1);
                int z = Map.GetAverageZ(x, y);

                Point3D loc = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(loc) && !Region.Find(loc, Map).IsPartOf("Khaldun"))
                {
                    return loc;
                }
            }

            return Point3D.Zero;
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
