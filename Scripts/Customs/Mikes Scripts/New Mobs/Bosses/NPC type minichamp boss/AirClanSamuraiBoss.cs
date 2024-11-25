using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the air clan samurai lord")]
    public class AirClanSamuraiBoss : AirClanSamurai
    {
        [Constructable]
        public AirClanSamuraiBoss() : base()
        {
            Name = "Air Clan Samurai Lord";
            Title = "the Stormbringer"; // A fitting title for the boss

            // Enhance stats to match or exceed a boss-level NPC
            SetStr(600, 800); // Higher strength for more tanking capability
            SetDex(500, 600); // Enhanced dexterity to match the agile nature of the air samurai
            SetInt(250, 350); // Higher intelligence for better spells and tactics

            SetHits(8000, 12000); // Increased health to make the boss formidable

            SetDamage(85, 120); // Stronger damage range to pose a greater challenge

            SetSkill(SkillName.Bushido, 150.0, 180.0); // Increase Bushido skill for more advanced combat moves
            SetSkill(SkillName.Anatomy, 120.0, 150.0); // Increased Anatomy skill for better damage output
            SetSkill(SkillName.Fencing, 120.0, 150.0); // Higher Fencing skill for better sword fighting
            SetSkill(SkillName.Macing, 120.0, 150.0); // Enhance Macing for a versatile fighter
            SetSkill(SkillName.MagicResist, 250.0, 300.0); // Increased Magic Resist to resist powerful spells
            SetSkill(SkillName.Swords, 120.0, 150.0); // Higher sword skill for more devastating attacks
            SetSkill(SkillName.Tactics, 120.0, 150.0); // Improved Tactics for better strategic combat
            SetSkill(SkillName.Wrestling, 120.0, 150.0); // Wrestling skill boost for better grappling

            Fame = 25000; // Higher fame for a more prestigious boss
            Karma = 25000; // Positive karma as a powerful figure in the air clan

            VirtualArmor = 75; // Increase armor to make the boss tougher to defeat

            // Attach a random ability to the boss for added complexity
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

            // Add additional air-themed loot or better items for a boss fight
            PackGold(2000, 3000);
            // Air-themed or powerful gear could be added, such as a Windblade or Air-themed magical items
            AddLoot(LootPack.AosSuperBoss);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Customize the boss' speech or additional behavior for flair
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "I control the winds of battle!"); break;
                        case 1: this.Say(true, "You cannot withstand my storm!"); break;
                        case 2: this.Say(true, "Feel the power of the wind!"); break;
                        case 3: this.Say(true, "The tempest is my ally!"); break;
                    }

                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            amount = base.Damage(amount, from);

            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "I am the storm itself!"); break;
                        case 1: this.Say(true, "The winds obey me!"); break;
                        case 2: this.Say(true, "Feel the fury of the sky!"); break;
                        case 3: this.Say(true, "I strike with the power of the cyclone!"); break;
                    }

                }
            }

            return amount;
        }

        public AirClanSamuraiBoss(Serial serial) : base(serial)
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
        }
    }
}
