using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a cursed harbinger corpse")]
    public class CursedHarbinger : BaseCreature
    {
        private DateTime m_NextCurseOfWeakness;
        private DateTime m_NextMisfortune;
        private DateTime m_NextDespair;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CursedHarbinger()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed harbinger";
            Body = 9; // Daemon body
            Hue = 1471; // Unique hue for the Cursed Harbinger
            BaseSoundID = 357;

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
            ControlSlots = Core.SE ? 4 : 5;
        }

        public CursedHarbinger(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override double DispelDifficulty => 125.0;

        public override double DispelFocus => 45.0;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextCurseOfWeakness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMisfortune = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextDespair = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCurseOfWeakness)
                {
                    CastCurseOfWeakness();
                }

                if (DateTime.UtcNow >= m_NextMisfortune)
                {
                    CastMisfortune();
                }

                if (DateTime.UtcNow >= m_NextDespair)
                {
                    CastDespair();
                }
            }
        }

        private void CastCurseOfWeakness()
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel a curse weakening your strength and dexterity!");
                    m.SendLocalizedMessage(1042000); // "You have been cursed!"
                    m.RawStr = (int)(m.RawStr * 0.75);
                    m.RawDex = (int)(m.RawDex * 0.75);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed Harbinger casts a curse of weakness! *");
            m_NextCurseOfWeakness = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CastMisfortune()
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    int effect = Utility.Random(3);
                    switch (effect)
                    {
                        case 0:
                            m.SendMessage("You are struck by a sudden wave of confusion!");
                            m.SendLocalizedMessage(1042001); // "You are confused!"
                            m.Freeze(TimeSpan.FromSeconds(3));
                            break;
                        case 1:
                            m.SendMessage("A wave of fear washes over you!");
                            m.SendLocalizedMessage(1042002); // "You are frightened!"
                            m.Damage(10, this);
                            break;
                        case 2:
                            m.SendMessage("A wave of pain surges through you!");
                            m.SendLocalizedMessage(1042003); // "You feel intense pain!"
                            m.Damage(5, this);
                            break;
                    }
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed Harbinger causes misfortune! *");
            m_NextMisfortune = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void CastDespair()
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are overwhelmed with a feeling of despair!");
                    m.SendLocalizedMessage(1042004); // "You are engulfed in despair!"
                    m.Damage(15, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed Harbinger inflicts despair! *");
            m_NextDespair = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
