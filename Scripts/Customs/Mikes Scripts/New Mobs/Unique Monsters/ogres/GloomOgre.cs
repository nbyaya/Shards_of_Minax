using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a gloom ogre corpse")]
    public class GloomOgre : BaseCreature
    {
        private DateTime m_NextGloomGaze;
        private DateTime m_NextDarknessShroud;
        private DateTime m_NextDespairAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GloomOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gloom ogre";
            Body = 1; // Ogre body
            Hue = 2172; // Dark hue for the Gloom Ogre
			BaseSoundID = 427;

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

        public GloomOgre(Serial serial)
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
                    m_NextGloomGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarknessShroud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextDespairAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGloomGaze)
                {
                    GloomGaze();
                }

                if (DateTime.UtcNow >= m_NextDarknessShroud)
                {
                    DarknessShroud();
                }

                if (DateTime.UtcNow >= m_NextDespairAura)
                {
                    DespairAura();
                }
            }
        }

        private void GloomGaze()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gloom Ogre's gaze is terrifying! *");
            PlaySound(0x5B9); // Terrifying sound
            FixedEffect(0x37B9, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    if (Utility.RandomDouble() < 0.25) // 25% chance to cause fear
                    {
                        m.SendMessage("You are filled with dread and are momentarily paralyzed!");
                        m.Freeze(TimeSpan.FromSeconds(3)); // Temporarily paralyze the target
                    }
                }
            }

            m_NextGloomGaze = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DarknessShroud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gloom Ogre envelops the area in darkness! *");
            PlaySound(0x3F0); // Dark shroud sound
            FixedEffect(0x3798, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The darkness reduces your visibility and accuracy!");
                    // Reduce their hit chance and damage here if applicable
                }
            }

            m_NextDarknessShroud = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void DespairAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* An aura of despair radiates from the Gloom Ogre! *");
            PlaySound(0x5B0); // Despair aura sound
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You feel your strength and defense waning!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                    // Optionally reduce attack and defense further here
                }
            }

            m_NextDespairAura = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
