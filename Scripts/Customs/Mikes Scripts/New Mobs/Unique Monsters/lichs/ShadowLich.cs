using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shadow lich corpse")]
    public class ShadowLich : BaseCreature
    {
        private DateTime m_NextShadowStrike;
        private DateTime m_NextDarkVeil;
        private DateTime m_NextNightmareGaze;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public ShadowLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow lich";
            Body = 24;
            Hue = 2135; // Dark, shadowy hue
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

            // Example items; you may adjust these or add more.
            PackItem(new LichFormScroll());
            PackItem(new VengefulSpiritScroll());
            PackNecroReg(20, 30);

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public ShadowLich(Serial serial)
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
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextNightmareGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextDarkVeil)
                {
                    DarkVeil();
                }

                if (DateTime.UtcNow >= m_NextNightmareGaze)
                {
                    NightmareGaze();
                }
            }
        }

        private void ShadowStrike()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    target.Damage(damage, this);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Strike! *");
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void DarkVeil()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("A dark veil envelops you, obscuring your vision!");
                    m.FixedEffect(0x3799, 10, 15);
                }
            }
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Dark Veil! *");
            m_NextDarkVeil = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void NightmareGaze()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null)
                {
                    target.SendMessage("You are filled with terror by the Shadow Lich's gaze!");
                    target.Freeze(TimeSpan.FromSeconds(5));
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nightmare Gaze! *");
                    m_NextNightmareGaze = DateTime.UtcNow + TimeSpan.FromMinutes(1);
                }
            }
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
}
