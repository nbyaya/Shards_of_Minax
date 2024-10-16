using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Puck corpse")]
    public class Puck : BaseCreature
    {
        private DateTime m_NextPrank;
        private DateTime m_NextDisguise;
        private DateTime m_NextLaugh;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Puck()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Puck";
            Body = 723; // GreenGoblin body
            BaseSoundID = 0x45A; // Pixie sound
            Hue = 1585; // Unique hue (you can adjust this)

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

        public Puck(Serial serial)
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

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPrank = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDisguise = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextLaugh = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPrank)
                {
                    DoPrank();
                }

                if (DateTime.UtcNow >= m_NextDisguise)
                {
                    DoDisguise();
                }

                if (DateTime.UtcNow >= m_NextLaugh)
                {
                    DoLaugh();
                }
            }
        }

        private void DoPrank()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Player)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Casts a confusing spell *");
                target.Send(new MessageLocalizedAffix(target.Serial, target.Body, MessageType.Regular, 0x3B2, 3, 1154, "", AffixType.Prepend | AffixType.System, "Your controls are confused!", ""));
                target.Direction = (Direction)Utility.Random(8);
            }

            m_NextPrank = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Prank
        }

        private void DoDisguise()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Transforms *");
            BodyValue = Utility.RandomList(3, 0x190, 0x191, 400, 401);
            Hue = Utility.RandomSkinHue();

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                BodyValue = 723;
                Hue = 1161;
            });

            m_NextDisguise = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Disguise
        }

        private void DoLaugh()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Laughs maniacally *");
            PlaySound(0x55F);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendLocalizedMessage(1070696); // You have been stunned by a concussion blow!
                }
            }

            m_NextLaugh = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Laugh
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
