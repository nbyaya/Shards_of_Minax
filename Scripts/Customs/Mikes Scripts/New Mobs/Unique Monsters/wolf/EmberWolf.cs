using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ember wolf corpse")]
    public class EmberWolf : BaseCreature
    {
        private DateTime m_NextMoltenBurst;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public EmberWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ember wolf";
            Body = 23; // Dire Wolf body
            Hue = 2635; // Fiery hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public EmberWolf(Serial serial)
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
                    m_NextMoltenBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMoltenBurst)
                {
                    MoltenBurst();
                }
            }
        }

        private void FlameSwipe(Mobile target)
        {
            if (Utility.RandomDouble() < 0.3) // 30% chance to deal extra fire damage
            {
                int fireDamage = Utility.RandomMinMax(5, 10);
                target.Damage(fireDamage, this);
                target.SendMessage("You are scorched by the Ember Wolf's fiery swipe!");
                target.FixedEffect(0x3709, 10, 16);
            }
        }

        private void MoltenBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ember Wolf erupts in flames! *");
            PlaySound(0x228);
            FixedEffect(0x3709, 10, 16); // Adjusted parameters to match the expected method signature

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int fireDamage = Utility.RandomMinMax(10, 15);
                    m.Damage(fireDamage, this);
                    m.SendMessage("You are burned by the Ember Wolf's molten burst!");
                }
            }

            Random rand = new Random();
            m_NextMoltenBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 120)); // Randomize the next ability time
        }

        public override void OnDamage(int amount, Mobile from, bool isFatal)
        {
            if (amount > 0)
            {
                // Reflect some fire damage back to attacker
                int reflectDamage = (int)(amount * 0.2); // Reflect 20% of damage
                from.Damage(reflectDamage, this);
                from.SendMessage("The Ember Wolf's fiery skin burns you in return!");
                from.FixedEffect(0x3709, 10, 16);
            }

            base.OnDamage(amount, from, isFatal);
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
