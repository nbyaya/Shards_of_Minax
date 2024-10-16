using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a terra wisp corpse")]
    public class TerraWisp : BaseCreature
    {
        private DateTime m_NextNatureWrath;
        private DateTime m_NextEarthCloak;
        private bool m_IsCloaked;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TerraWisp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a terra wisp";
            this.Body = 14; // Use an elemental or foliage-like body
            this.Hue = 0x3B2; // Earthy color
			BaseSoundID = 268;

            this.SetStr(80, 100);
            this.SetDex(100, 120);
            this.SetInt(60, 80);

            this.SetHits(50, 75);

            this.SetDamage(12, 20);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.MagicResist, 60.0, 80.0);
            this.SetSkill(SkillName.Tactics, 50.0, 70.0);
            this.SetSkill(SkillName.Wrestling, 50.0, 70.0);

            this.VirtualArmor = 30;
            this.ControlSlots = 2;

            this.PackItem(new FertileDirt(Utility.RandomMinMax(1, 2)));
            this.PackItem(new MandrakeRoot());

            Item ore = new IronOre(3);
            ore.ItemID = 0x19B7;
            this.PackItem(ore);

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public TerraWisp(Serial serial)
            : base(serial)
        {
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
                    m_NextNatureWrath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEarthCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNatureWrath)
                {
                    NatureWrath();
                }

                if (DateTime.UtcNow >= m_NextEarthCloak && m_IsCloaked)
                {
                    // Deactivate Earth Cloak after 10 seconds
                    m_IsCloaked = false;
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The earth cloak fades away *");
                }
            }
        }

        private void NatureWrath()
        {
            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Player)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 0);
                    m.SendMessage("You are struck by a burst of nature energy!");
                    
                    Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(() =>
                    {
                        if (m.Alive)
                            m.SendMessage("The nature energy wears off, but your attack speed is still reduced.");
                    }));
                }
            }

            m_NextNatureWrath = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Reset cooldown
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (!m_IsCloaked && willKill)
            {
                EarthCloak();
            }
        }

        private void EarthCloak()
        {
            m_IsCloaked = true;
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The earth cloak surrounds the wisp *");

            // Add a 20% dodge chance (implementation needed)
            
            // Set next cloak deactivation time
            m_NextEarthCloak = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Reset cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_IsCloaked);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsCloaked = reader.ReadBool();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
