using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bonecrusher ogre corpse")]
    public class BonecrusherOgre : BaseCreature
    {
        private DateTime m_NextBoneShatter;
        private DateTime m_NextStunningRoar;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BonecrusherOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bonecrusher ogre";
            Body = 1; // Ogre body
            Hue = 2175; // Dark hue for uniqueness
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

            PackItem(new Club());

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public BonecrusherOgre(Serial serial)
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
                    m_NextBoneShatter = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStunningRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBoneShatter)
                {
                    BoneShatter();
                }

                if (DateTime.UtcNow >= m_NextStunningRoar)
                {
                    StunningRoar();
                }
            }
        }

        private void BoneShatter()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bone Shatter! *");
                FixedEffect(0x376A, 10, 16);
                
                target.SendMessage("Your bones crack under the force of the ogre's attack!");
                target.Damage(Utility.RandomMinMax(10, 15), this);
                target.SendMessage("Your defense is weakened!");

                // Reduce target's defense (armor) temporarily
                target.VirtualArmor -= 10;
                if (target.VirtualArmor < 0) target.VirtualArmor = 0;

                m_NextBoneShatter = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void StunningRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Stunning Roar! *");
            FixedEffect(0x37C4, 10, 36);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    m.SendMessage("You are stunned by the ogre's roar!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextStunningRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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
