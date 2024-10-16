using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a chamois corpse")]
    public class Chamois : BaseCreature
    {
        private DateTime m_NextQuickReflexes;
        private DateTime m_NextCamouflage;
        private DateTime m_NextChargeAttack;
        private DateTime m_NextBouncingLeap;
        private DateTime m_NextTerritorialRoar;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Chamois()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a chamois";
            Body = 0xD1; // Goat body
            Hue = 1919; // Reddish-brown hue
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

        public Chamois(Serial serial)
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
                    m_NextQuickReflexes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextChargeAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextBouncingLeap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextTerritorialRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextQuickReflexes)
                {
                    QuickReflexes();
                }

                if (DateTime.UtcNow >= m_NextCamouflage)
                {
                    MountainCamouflage();
                }

                if (DateTime.UtcNow >= m_NextChargeAttack)
                {
                    ChargeAttack();
                }

                if (DateTime.UtcNow >= m_NextBouncingLeap)
                {
                    BouncingLeap();
                }

                if (DateTime.UtcNow >= m_NextTerritorialRoar)
                {
                    TerritorialRoar();
                }
            }
        }

        private void QuickReflexes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chamois moves with incredible speed and precision! *");
            this.Dex += 30; // Increase dexterity temporarily

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(ResetQuickReflexes));
            m_NextQuickReflexes = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ResetQuickReflexes()
        {
            this.Dex -= 30; // Reset dexterity
        }

        private void MountainCamouflage()
        {
            if (OnElevatedTerrain())
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chamois blends into the rocky terrain, becoming nearly invisible! *");
                this.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(ResetCamouflage));
                m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void ResetCamouflage()
        {
            this.Hidden = false;
        }

        private void ChargeAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chamois charges at you with immense force! *");
            if (Combatant != null && !Combatant.Deleted && InRange(Combatant, 5))
            {
                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
                Combatant.PlaySound(0x1F2); // Charging sound effect
            }
            m_NextChargeAttack = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void BouncingLeap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chamois leaps and lands with a thunderous impact! *");
            if (Combatant != null)
            {
                Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-3, 3), Y + Utility.RandomMinMax(-3, 3), Z);
                if (Map.CanSpawnMobile(newLocation))
                {
                    MoveToWorld(newLocation, Map);
                    Effects.SendLocationEffect(newLocation, Map, 0x36D4, 10, 16); // Impact effect

                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m != Combatant && m.Alive)
                        {
                            int damage = Utility.RandomMinMax(5, 10);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                            m.SendMessage("You are knocked back by the Chamois's impact!");
                        }
                    }
                }
            }
            m_NextBouncingLeap = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void TerritorialRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chamois lets out a deafening roar, boosting its own strength! *");
            this.RawStr += 20; // Increase strength temporarily
            this.VirtualArmor += 10; // Increase armor temporarily

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(ResetTerritorialRoar));
            m_NextTerritorialRoar = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void ResetTerritorialRoar()
        {
            this.RawStr -= 20; // Reset strength
            this.VirtualArmor -= 10; // Reset armor
        }

        private bool OnElevatedTerrain()
        {
            // Check if the Chamois is on elevated terrain
            return (Z > Map.GetAverageZ(X, Y) + 5); // Example check for elevation
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
