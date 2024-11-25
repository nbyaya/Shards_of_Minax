using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the drum overlord")]
    public class DrummerBoss : Drummer
    {
        private TimeSpan m_BuffDelay = TimeSpan.FromSeconds(10.0); // Reduced buff delay for more frequent buffs
        public DateTime m_NextBuffTime;
        private bool m_BuffActive = false;

        [Constructable]
        public DrummerBoss() : base()
        {
            Name = "Drum Overlord";
            Title = "the Rhythmic Master";

            // Enhance stats to match or exceed Barracoon's stats
            SetStr(700, 900); // Increased strength for higher durability
            SetDex(150, 200); // Increased dexterity for better evasion and speed
            SetInt(75, 100); // Increased intelligence for better skill usage

            SetHits(12000); // Increased health to match a boss-tier creature
            SetDamage(15, 30); // Higher damage output

            SetResistance(ResistanceType.Physical, 60, 75); // Increased resistances for higher durability
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Increased skill range for a more challenging encounter
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 22500; // Increased fame to signify a boss-level creature
            Karma = -22500; // Increased karma to show the negative impact of the boss

            VirtualArmor = 50; // Increased armor for higher defense

            // Attach the XmlRandomAbility for added unpredictability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextBuffTime = DateTime.Now + m_BuffDelay;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new FocusAugmentCrystal());
			PackItem(new FishermansSunHat());           
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Shorter delay for buffs to make it a more dynamic fight
            if (DateTime.Now >= m_NextBuffTime && !m_BuffActive)
            {
                Effects.PlaySound(Location, Map, 0x1F7); // Drum sound effect
                Say(true, "Feel the rhythm!");

                foreach (Mobile m in GetMobilesInRange(8))
                {
                    if (m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        bc.AddStatMod(new StatMod(StatType.Str, "DrumBuff", 20, TimeSpan.FromSeconds(30.0)));
                        bc.AddStatMod(new StatMod(StatType.Dex, "DrumBuff", 20, TimeSpan.FromSeconds(30.0)));
                        bc.AddStatMod(new StatMod(StatType.Int, "DrumBuff", 20, TimeSpan.FromSeconds(30.0)));
                    }
                }

                m_BuffActive = true;
                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }
            else if (DateTime.Now >= m_NextBuffTime && m_BuffActive)
            {
                m_BuffActive = false;
                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }
        }

        public DrummerBoss(Serial serial) : base(serial)
        {
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
        }
    }
}
