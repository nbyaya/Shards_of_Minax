using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Graffiti Overlord")]
    public class GraffitiGargoyleBoss : GraffitiGargoyle
    {
        [Constructable]
        public GraffitiGargoyleBoss() : base()
        {
            Name = "Graffiti Overlord";
            Title = "the Master of Walls";

            // Enhance stats to match or exceed a boss-tier creature
            SetStr(1200);  // Upper strength value, enhanced for boss
            SetDex(255);   // Upper dexterity value
            SetInt(250);   // Upper intelligence value

            SetHits(12000); // Enhanced health for a boss
            SetDamage(29, 38); // Matching a higher damage output

            // Enhance resistances to make the boss tougher
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhance skills to reflect the strength of a boss
            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Increased virtual armor for higher survivability

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Make sure the boss drops 5 MaxxiaScrolls
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Add specific loot for the boss, including MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add more valuable loot for a boss
            PackGold(500, 800);
            AddLoot(LootPack.FilthyRich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Add custom behavior for the boss, such as unique speech or attacks
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Your end is painted on the wall!"); break;
                        case 1: this.Say(true, "I foresee your doom!"); break;
                        case 2: this.Say(true, "The stone speaks of your fate!"); break;
                        case 3: this.Say(true, "Every stroke leads to your demise!"); break;
                    }

                }
            }
        }

        public GraffitiGargoyleBoss(Serial serial) : base(serial)
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
