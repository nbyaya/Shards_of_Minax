using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a shadow anaconda corpse")]
    public class ShadowAnaconda : BaseCreature
    {
        private static readonly int[] AbilityHue = new int[] { 0x45B, 0x47E }; // Dark hues for shadow effects

        private DateTime m_NextShadowCloak;
        private DateTime m_NextSuffocatingWrap;
        private DateTime m_NextDarkVenom;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowAnaconda()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Shadow Anaconda";
            Body = 0x15; // Giant Serpent body
            Hue = 1772; // Dark green hue for the Shadow Anaconda
			BaseSoundID = 219;

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

        public ShadowAnaconda(Serial serial)
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
                    m_NextShadowCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSuffocatingWrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkVenom = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowCloak)
                {
                    UseShadowCloak();
                }

                if (DateTime.UtcNow >= m_NextSuffocatingWrap)
                {
                    UseSuffocatingWrap();
                }

                if (DateTime.UtcNow >= m_NextDarkVenom)
                {
                    UseDarkVenom();
                }
            }
        }

        private void UseShadowCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Anaconda vanishes into shadows! *");
            this.Hue = AbilityHue[0];
            this.Hidden = true;
            this.VirtualArmor += 20; // Increase virtual armor for better defense

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(RevealShadowCloak));
            m_NextShadowCloak = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown of 1 minute
        }

        private void RevealShadowCloak()
        {
            this.Hidden = false;
            this.Hue = 0x1F4; // Revert to original hue
            this.VirtualArmor -= 20; // Reset virtual armor
        }

        private void UseSuffocatingWrap()
        {
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* You are suffocated by the Shadow Anaconda! *");
                    m.SendMessage("You are constricted and stunned by the Shadow Anaconda!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 0, 100, 0, 0, 0);

                    // Apply debuff: Reduce strength and dexterity
                    m.Damage(5, this);
                    m.SendMessage("You feel weakened by the Shadow Anaconda's constriction.");
                }
            }

            m_NextSuffocatingWrap = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown of 30 seconds
        }

        private void UseDarkVenom()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Dark venom courses through your veins! *");
                    m.SendMessage("You are poisoned by the Dark Venom of the Shadow Anaconda!");
                    
                    // Apply dark poison
                    Poison poison = Poison.Greater;
                    m.ApplyPoison(this, poison);
                    
                    // Additional poison effects
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
                    {
                        if (!m.Deleted && m.Alive)
                        {
                            m.SendMessage("The Dark Venom's effects intensify!");
                            AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                        }
                    });
                }
            }

            m_NextDarkVenom = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown of 20 seconds
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Special effect on death
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Anaconda disintegrates into shadows! *");
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10); // Dark explosion effect
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
