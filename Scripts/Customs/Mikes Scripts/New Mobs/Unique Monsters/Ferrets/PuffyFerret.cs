using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a puffy ferret corpse")]
    public class PuffyFerret : BaseCreature
    {
        private static readonly string[] m_Vocabulary = new string[]
        {
            "puff puff",
            "so fluffy!",
            "feel the fluff!"
        };

        private DateTime m_NextPuffOfJoy;
        private DateTime m_NextFluffShield;
        private DateTime m_NextFluffyExplosion;
        private DateTime m_NextFurryTeleport;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public PuffyFerret()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a puffy ferret";
            Body = 0x117; // Using ferret body as base
            Hue = 1570; // Unique hue for Puffy Ferret
			BaseSoundID = 0xCF;

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

            m_AbilitiesInitialized = false;
        }

        public PuffyFerret(Serial serial)
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
                    m_NextPuffOfJoy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFluffShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFluffyExplosion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextFurryTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPuffOfJoy)
                {
                    PuffOfJoy();
                }

                if (DateTime.UtcNow >= m_NextFluffShield)
                {
                    FluffShield();
                }

                if (DateTime.UtcNow >= m_NextFluffyExplosion)
                {
                    FluffyExplosion();
                }

                if (DateTime.UtcNow >= m_NextFurryTeleport)
                {
                    FurryTeleport();
                }
            }
        }

        private void PuffOfJoy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a puff of comforting air *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature)
                {
                    creature.Heal(15);
                    creature.Mana += 15;
                    m.SendMessage("You feel comforted by the Puffy Ferret's puff of joy!");
                }
            }

            m_NextPuffOfJoy = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Fixed cooldown for consistency
        }

        private void FluffShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Envelopes itself in a protective layer of fluff *");
            FixedEffect(0x37C4, 10, 36);

            this.VirtualArmor += 30;

            Timer.DelayCall(TimeSpan.FromSeconds(15), () => { this.VirtualArmor -= 30; });

            m_NextFluffShield = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown for consistency
        }

        private void FluffyExplosion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a fluffy explosion *");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this);
                    m.SendMessage("You are hit by a fluffy explosion!");
                }
            }

            m_NextFluffyExplosion = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown for consistency
        }

        private void FurryTeleport()
        {
            if (Combatant == null)
                return;

            Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-10, 10), Y + Utility.RandomMinMax(-10, 10), Z);
            if (Map.CanSpawnMobile(newLocation))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Teleports with a burst of fluff *");
                FixedEffect(0x376A, 10, 16);

                MoveToWorld(newLocation, Map);

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("The Puffy Ferret disappears in a puff of fluff and reappears somewhere else!");
                    }
                }
            }

            m_NextFurryTeleport = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Fixed cooldown for consistency
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
