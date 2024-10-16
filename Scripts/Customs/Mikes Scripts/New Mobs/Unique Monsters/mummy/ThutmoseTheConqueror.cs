using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Thutmose the Conqueror corpse")]
    public class ThutmoseTheConqueror : BaseCreature
    {
        private DateTime m_NextRoar;
        private DateTime m_NextWarriorsSpirit;
        private DateTime m_NextCurse;
        private DateTime m_NextMummification;
        private DateTime m_NextSummonUndead;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ThutmoseTheConqueror()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "Thutmose the Conqueror";
            Body = 154; // Mummy body
            Hue = 2159; // Unique hue for Thutmose
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

        public ThutmoseTheConqueror(Serial serial)
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
                    m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextWarriorsSpirit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMummification = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSummonUndead = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRoar)
                {
                    ConqueringRoar();
                }

                if (DateTime.UtcNow >= m_NextWarriorsSpirit)
                {
                    WarriorsSpirit();
                }

                if (DateTime.UtcNow >= m_NextCurse)
                {
                    PharaohsCurse();
                }

                if (DateTime.UtcNow >= m_NextMummification)
                {
                    Mummification();
                }

                if (DateTime.UtcNow >= m_NextSummonUndead)
                {
                    SummonUndead();
                }
            }
        }

        private void ConqueringRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thutmose’s roar shakes the very earth beneath you! *");
            PlaySound(0x208); // Roar sound

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    // Reduce combat effectiveness of enemies within range
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                    m.SendMessage("You feel weakened by Thutmose's mighty roar!");
                }
            }

            m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ConqueringRoar
        }

        private void WarriorsSpirit()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thutmose channels the Warrior’s Spirit, empowering himself with fierce strength! *");
            PlaySound(0x1F2); // Power-up sound

            // Increase Thutmose’s combat stats temporarily
            SetStr(500, 550);
            SetDex(100, 120);
            SetInt(60, 80);

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                // Restore stats after 10 seconds
                SetStr(400, 450);
                SetDex(80, 100);
                SetInt(40, 60);
            });

            m_NextWarriorsSpirit = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for WarriorsSpirit
        }

        private void PharaohsCurse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thutmose curses you with Pharaoh's wrath! *");
            PlaySound(0x1C1); // Curse sound

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    // Apply a debuff to reduce the player's combat effectiveness
                    m.SendMessage("You are cursed by Thutmose, feeling a great weight upon you!");
                }
            }

            m_NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for PharaohsCurse
        }

        private void Mummification()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thutmose wraps you in cursed bandages! *");
            PlaySound(0x1F2); // Mummification sound

            if (Combatant != null)
            {
                // Temporarily immobilize the target
				Mobile mobile = Combatant as Mobile;
				if (mobile != null)
				{
					mobile.SendMessage("You are temporarily immobilized by Thutmose's mummification!");
					mobile.Freeze(TimeSpan.FromSeconds(5));
				}
            }

            m_NextMummification = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Mummification
        }

        private void SummonUndead()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thutmose summons the power of the undead! *");
            PlaySound(0x2D8); // Summoning sound

            for (int i = 0; i < 2; i++)
            {
                Mummy minion = new Mummy();
                minion.Team = this.Team; // Ensure the minions are on Thutmose's team
                minion.MoveToWorld(Location, Map);
                minion.Combatant = Combatant; // Assign combatant
            }

            m_NextSummonUndead = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Cooldown for SummonUndead
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

    public class Debuff
    {
        public enum DebuffType { Curse }
        private DebuffType m_Type;
        private TimeSpan m_Duration;

        public Debuff(DebuffType type, TimeSpan duration)
        {
            m_Type = type;
            m_Duration = duration;
        }

        public void Apply(Mobile target)
        {
            // Apply the debuff effect to the target
            // This method would need to be implemented based on how debuffs are handled in your server
        }
    }
}
