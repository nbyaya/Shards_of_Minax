using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a Ramses the Immortal corpse")]
    public class RamsesTheImmortal : BaseCreature
    {
        private DateTime m_NextImmortalShield;
        private DateTime m_NextDivineStrike;
        private DateTime m_NextSummonPharaohs;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RamsesTheImmortal()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Ramses the Immortal";
            Body = 154; // Mummy body
            Hue = 2161; // Unique hue (golden color)
			BaseSoundID = 471;

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

        public RamsesTheImmortal(Serial serial)
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
                    m_NextImmortalShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDivineStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextSummonPharaohs = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextImmortalShield)
                {
                    ImmortalShield();
                }

                if (DateTime.UtcNow >= m_NextDivineStrike)
                {
                    DivineStrike();
                }

                if (DateTime.UtcNow >= m_NextSummonPharaohs)
                {
                    SummonPharaohs();
                }
            }
        }

        private void ImmortalShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ramses invokes the Immortal Shield! *");
            PlaySound(0x20D); // Shield sound

            // Apply shield effect
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            this.VirtualArmor += 30; // Increase virtual armor

            // Shield duration
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => RemoveImmortalShield());

            m_NextImmortalShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for ImmortalShield
        }

        private void RemoveImmortalShield()
        {
            this.VirtualArmor -= 30; // Revert armor increase
        }

        private void DivineStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ramses strikes with divine fury! *");
            PlaySound(0x20F); // Strike sound

            // Apply Divine Strike effect
            if (Combatant != null)
            {
                AOS.Damage(Combatant, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);

                if (Utility.RandomDouble() < 0.25) // 25% chance to blind
                {
					Mobile mobile = Combatant as Mobile;
					if (mobile != null)
					{
						mobile.SendMessage("You are blinded by the divine light!");
						mobile.Paralyze(TimeSpan.FromSeconds(5)); // Blinding effect
					}
                }
            }

            m_NextDivineStrike = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DivineStrike
        }

        private void SummonPharaohs()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ramses summons the Pharaohs of the past! *");
            PlaySound(0x20F); // Summon sound

            // Summon 1-3 additional Mummy creatures to fight alongside Ramses
            int pharaohCount = Utility.RandomMinMax(1, 3);
            for (int i = 0; i < pharaohCount; i++)
            {
                Mummy pharaoh = new Mummy
                {
                    Name = "Pharaoh's Minion",
                    Hue = 1153 // Match Ramses's hue
                };
                pharaoh.MoveToWorld(this.Location, this.Map);
            }

            m_NextSummonPharaohs = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for SummonPharaohs
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
