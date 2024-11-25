using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a venom bear overlord corpse")]
    public class VenomBearBoss : VenomBear
    {
        private DateTime m_NextToxicSpit;
        private DateTime m_NextPoisonousAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VenomBearBoss()
            : base()
        {
            Name = "Venom Bear Overlord";
            Title = "the Toxic Terror";

            // Enhance the stats to match or exceed Barracoon's values
            SetStr(1200); // Higher strength
            SetDex(255);  // Higher dexterity
            SetInt(250);  // Higher intelligence

            SetHits(15000); // Increased health

            SetDamage(45, 55); // Stronger damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 120;

            Tamable = false;  // Bosses are usually untameable
            ControlSlots = 0;

            m_AbilitiesInitialized = false; // Set the flag to false

            // Attach the XmlRandomAbility attachment
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
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
                    m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPoisonousAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextToxicSpit)
                {
                    ToxicSpit();
                }

                if (DateTime.UtcNow >= m_NextPoisonousAura)
                {
                    PoisonousAura();
                }
            }
        }

        private void ToxicSpit()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile;
                int damage = Utility.RandomMinMax(30, 45); // Stronger Toxic Spit
                target.Damage(damage, this);
                target.SendMessage("You have been hit by the Venom Bear Overlord's toxic spit!");
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Venom Bear Overlord spits toxic poison *");
                target.FixedEffect(0x376A, 10, 16); // Green toxic visual effect

                m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Shortened cooldown for ToxicSpit
            }
        }

        private void PoisonousAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Venom Bear Overlord emits a poisonous aura *");
            FixedEffect(0x376A, 10, 16); // Green mist visual effect

            foreach (Mobile m in GetMobilesInRange(6)) // Increased range for Poisonous Aura
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(10, 20); // Stronger aura damage
                    m.Damage(damage, this);
                    m.SendMessage("You are poisoned by the Venom Bear Overlord's aura!");
                }
            }

            m_NextPoisonousAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Shortened cooldown for PoisonousAura
        }

        public VenomBearBoss(Serial serial)
            : base(serial)
        {
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
