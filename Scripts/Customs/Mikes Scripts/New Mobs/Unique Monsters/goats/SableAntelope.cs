using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a sable antelope corpse")]
    public class SableAntelope : BaseCreature
    {
        private DateTime m_NextSwiftStrike;
        private DateTime m_NextGracefulDodge;
        private DateTime m_NextAntelopeCharge;
        private DateTime m_NextTeleport;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public SableAntelope()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Sable Antelope";
            Body = 0xD1; // Using goat body
            Hue = 1804; // Tan hue
			BaseSoundID = 0x99;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public SableAntelope(Serial serial)
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
                    m_NextSwiftStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGracefulDodge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextAntelopeCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextSwiftStrike)
                {
                    SwiftStrike();
                }

                if (DateTime.UtcNow >= m_NextGracefulDodge)
                {
                    GracefulDodge();
                }

                if (DateTime.UtcNow >= m_NextAntelopeCharge)
                {
                    AntelopeCharge();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }
            }
        }

        private void SwiftStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sable Antelope strikes with lightning speed!*");

            if (Combatant != null)
            {
                int extraDamage = Utility.RandomMinMax(10, 15);
                AOS.Damage(Combatant, this, extraDamage, 0, 100, 0, 0, 0);

                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    if (Utility.RandomDouble() < 0.25) // 25% chance to stun
                    {
                        target.Freeze(TimeSpan.FromSeconds(2));
                        target.SendMessage("You are stunned by the Sable Antelope's swift strike!");
                    }
                }
            }

            m_NextSwiftStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset to fixed cooldown
        }

        private void GracefulDodge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sable Antelope moves with unparalleled grace!*");
            this.Hidden = true; // Temporary invisibility
            Timer.DelayCall(TimeSpan.FromSeconds(5), () => 
            {
                if (!Deleted)
                    this.Hidden = false;
            });

            m_NextGracefulDodge = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset to fixed cooldown
        }

        private void AntelopeCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sable Antelope charges with great force!*");

            if (Combatant != null)
            {
                int distance = 2;
                Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-distance, distance), Y + Utility.RandomMinMax(-distance, distance), Z);
                if (Map.CanSpawnMobile(newLocation))
                {
                    MoveToWorld(newLocation, Map);
                    FixedEffect(0x376A, 10, 16);

                    Mobile target = Combatant as Mobile;
                    if (target != null)
                    {
                        target.SendMessage("You are knocked back by the Sable Antelope's charge!");
                    }
                }
            }

            m_NextAntelopeCharge = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Reset to fixed cooldown
        }

        private void Teleport()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sable Antelope swiftly teleports away!*");
            Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-10, 10), Y + Utility.RandomMinMax(-10, 10), Z);
            if (Map.CanSpawnMobile(newLocation))
            {
                MoveToWorld(newLocation, Map);
                FixedEffect(0x3728, 10, 30);
            }

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset to fixed cooldown
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
            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
        }
    }
}
