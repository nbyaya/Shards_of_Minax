using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a thul'gor the forsaken corpse")]
    public class ThulGorTheForsaken : BaseCreature
    {
        private DateTime m_NextAbyssalHowl;
        private DateTime m_NextShadowBind;
        private DateTime m_NextDarkPact;
        private bool m_IsDarkPactActive;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ThulGorTheForsaken()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Thul'Gor the Forsaken";
            Body = 22; // ElderGazer body
            Hue = 1766; // Dark purple hue
			BaseSoundID = 377;

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

        public ThulGorTheForsaken(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextAbyssalHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowBind = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextDarkPact = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAbyssalHowl)
                {
                    AbyssalHowl();
                }

                if (DateTime.UtcNow >= m_NextShadowBind)
                {
                    ShadowBind();
                }

                if (DateTime.UtcNow >= m_NextDarkPact && !m_IsDarkPactActive)
                {
                    DarkPact();
                }
            }
        }

        private void AbyssalHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thul'Gor lets out a bone-chilling Abyssal Howl! *");
            PlaySound(0x20F);
            FixedEffect(0x37C4, 10, 20);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are struck with fear by Thul'Gor's Abyssal Howl!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Fear effect
                }
            }

            m_NextAbyssalHowl = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ShadowBind()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadowy tendrils erupt from Thul'Gor, binding his enemies! *");
            PlaySound(0x20F);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are restrained by shadowy tendrils!");
                    m.Freeze(TimeSpan.FromSeconds(5)); // Immobilization effect
                }
            }

            m_NextShadowBind = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DarkPact()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thul'Gor makes a Dark Pact, empowering himself at a cost! *");
            PlaySound(0x20F);
            FixedEffect(0x37C4, 10, 36);

            if (Hits > 50)
            {
                Hits -= 50; // Sacrificing health
                SetStr(Str + 50);
                SetDex(Dex + 30);
                SetInt(Int + 30);
                VirtualArmor += 20;
                m_IsDarkPactActive = true;

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(EndDarkPact));
            }

            m_NextDarkPact = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void EndDarkPact()
        {
            if (m_IsDarkPactActive)
            {
                SetStr(Str - 50);
                SetDex(Dex - 30);
                SetInt(Int - 30);
                VirtualArmor -= 20;
                m_IsDarkPactActive = false;
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
