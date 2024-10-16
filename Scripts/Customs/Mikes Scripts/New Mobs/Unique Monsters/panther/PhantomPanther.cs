using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a phantom panther corpse")]
    public class PhantomPanther : BaseCreature
    {
        private DateTime m_NextSpectralSwipe;
        private DateTime m_NextGhostWalk;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PhantomPanther()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a phantom panther";
            Body = 0xD6; // Panther body
            Hue = 2180; // Ghostly blue hue
            BaseSoundID = 0x462; // Panther sound

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

        public PhantomPanther(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSpectralSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random start time for SpectralSwipe
                    m_NextGhostWalk = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random start time for GhostWalk
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSpectralSwipe)
                {
                    SpectralSwipe();
                }

                if (DateTime.UtcNow >= m_NextGhostWalk)
                {
                    GhostWalk();
                }
            }
        }

        private void SpectralSwipe()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ethereal claws materialize *");
            PlaySound(0x56D);

            if (Combatant is Mobile target && target.Alive)
            {
                target.PlaySound(0x1F1);
                target.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);

                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                target.SendLocalizedMessage(1070793); // The panther's ethereal claws phase through your armor!
            }

            m_NextSpectralSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Reset cooldown
        }

        private void GhostWalk()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Phases through reality *");
            PlaySound(0x482);
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && m.Player)
                {
                    m.SendLocalizedMessage(1070794); // The phantom panther passes through you, chilling you to the bone!
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x1EA);

                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                }
            }

            m_NextGhostWalk = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
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
