using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ancient wolf corpse")]
    public class AncientWolf : BaseCreature
    {
        private DateTime m_NextAncientRoar;
        private DateTime m_NextMysticProtection;
        private DateTime m_NextSpiritWalk;
        private bool m_HasShield;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public AncientWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ancient wolf";
            Body = 23; // DireWolf body
            Hue = 2645; // Unique hue for the ancient wolf
            BaseSoundID = 0xE5;

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

            m_AbilitiesInitialized = false; // Set the flag to false initially
        }

        public AncientWolf(Serial serial)
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
                    // Initialize random start times for abilities
                    Random rand = new Random();
                    m_NextAncientRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMysticProtection = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSpiritWalk = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initialization
                }

                if (DateTime.UtcNow >= m_NextAncientRoar)
                {
                    AncientRoar();
                }

                if (DateTime.UtcNow >= m_NextMysticProtection)
                {
                    MysticProtection();
                }

                if (DateTime.UtcNow >= m_NextSpiritWalk)
                {
                    SpiritWalk();
                }
            }
        }

        private void AncientRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ancient Roar! *");
            PlaySound(0x1F5);
            FixedEffect(0x3728, 10, 16);

            SetStr(Str + 20);
            SetDex(Dex + 10);
            SetInt(Int + 15);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature && ((BaseCreature)m).IsMonster)
                {
                    m.PlaySound(0x1F6);
                    m.FixedEffect(0x375A, 10, 16);
                }
                else if (m != this && m.Player)
                {
                    m.SendMessage("You feel a chill as the Ancient Wolf roars!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextAncientRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for AncientRoar
        }

        private void MysticProtection()
        {
            if (m_HasShield)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mystic Shield Activated! *");
            PlaySound(0x1F4);
            FixedEffect(0x374A, 10, 16);

            m_HasShield = true;

            Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerCallback(() =>
            {
                m_HasShield = false;
            }));

            m_NextMysticProtection = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for MysticProtection
        }

        private void SpiritWalk()
        {
            Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
            if (Map.CanSpawnMobile(newLocation))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Spirit Walk! *");
                MoveToWorld(newLocation, Map);
                FixedEffect(0x3728, 10, 16);
                PlaySound(0x1F7);

                m_NextSpiritWalk = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for SpiritWalk
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_HasShield);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_HasShield = reader.ReadBool();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
