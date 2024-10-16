using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an ish'kar the forgotten corpse")]
    public class IshKarTheForgotten : ElderGazer
    {
        private DateTime m_NextArcaneBlast;
        private DateTime m_NextKnowledgeDrain;
        private DateTime m_NextForgottenCurse;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IshKarTheForgotten()
            : base()
        {
            Name = "Ish'Kar the Forgotten";
            Hue = 0x482; // Unique hue for Ish'Kar
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

        public IshKarTheForgotten(Serial serial)
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
                    m_NextArcaneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextKnowledgeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextForgottenCurse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextArcaneBlast)
                {
                    ArcaneBlast();
                }

                if (DateTime.UtcNow >= m_NextKnowledgeDrain)
                {
                    KnowledgeDrain();
                }

                if (DateTime.UtcNow >= m_NextForgottenCurse)
                {
                    ForgottenCurse();
                }
            }
        }

        private void ArcaneBlast()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    // Define the damage types for Arcane Blast
                    int fireDamage = Utility.RandomMinMax(10, 20);
                    int coldDamage = Utility.RandomMinMax(10, 20);
                    int energyDamage = Utility.RandomMinMax(10, 20);

                    target.Damage(fireDamage, this);
                    target.Damage(coldDamage, this);
                    target.Damage(energyDamage, this);

                    // Effects
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ish'Kar unleashes a devastating Arcane Blast! *");
                    target.SendMessage("You are struck by a powerful arcane force!");

                    FixedEffect(0x376A, 10, 16); // Example effect
                    PlaySound(0x20B); // Example sound

                    m_NextArcaneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset cooldown
                }
            }
        }

        private void KnowledgeDrain()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("Ish'Kar drains your knowledge, making your spells less effective!");

                    // Example effect
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ish'Kar drains your magical knowledge! *");
                    FixedEffect(0x376A, 10, 16);

                    // Reduce skill effectiveness (example - adjust as needed)

                    m_NextKnowledgeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
                }
            }
        }

        private void ForgottenCurse()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.SendMessage("A curse from Ish'Kar lowers your resistance to magic!");

                    // Example effect
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ish'Kar casts Forgotten Curse upon you! *");
                    FixedEffect(0x376A, 10, 16);

                    // Increase susceptibility to spells (example - adjust as needed)

                    m_NextForgottenCurse = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset cooldown
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // No need to serialize the cooldowns as they will be reset
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the flag to reinitialize abilities
        }
    }
}
