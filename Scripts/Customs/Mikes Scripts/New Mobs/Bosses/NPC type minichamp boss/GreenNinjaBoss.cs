using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the green ninja overlord")]
    public class GreenNinjaBoss : GreenNinja
    {
        [Constructable]
        public GreenNinjaBoss() : base()
        {
            Name = "Green Ninja Overlord";
            Title = "the Master of Shadows";

            // Enhance stats to match or exceed the boss tier
            SetStr(850); // Upper limit of strength
            SetDex(600); // Upper limit of dexterity
            SetInt(600); // Upper limit of intelligence

            SetHits(1400); // Increased health to match the boss tier
            SetDamage(130, 150); // Enhanced damage range

            // Increase resistances to be more resilient
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 60);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 60);

            // Enhance skills to match a boss
            SetSkill(SkillName.Anatomy, 120.0, 200.0);
            SetSkill(SkillName.Fencing, 200.0);
            SetSkill(SkillName.Macing, 200.0);
            SetSkill(SkillName.MagicResist, 200.0);
            SetSkill(SkillName.Swords, 200.0);
            SetSkill(SkillName.Tactics, 200.0);
            SetSkill(SkillName.Wrestling, 200.0);
            SetSkill(SkillName.Ninjitsu, 200.0);

            Fame = 25000; // Increased fame for a boss-level character
            Karma = -2500; // Slightly negative karma, fitting for a ninja overlord

            // Add random ability attachment for dynamic difficulty
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

            // Include ninja-themed loot like throwing stars or smoke bombs
            PackItem(new SmokeBomb(Utility.RandomMinMax(1, 5)));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Custom speech pattern for the boss version
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The shadows will consume you."); break;
                        case 1: this.Say(true, "No one escapes the silent blade."); break;
                        case 2: this.Say(true, "Feel the weight of my vengeance."); break;
                        case 3: this.Say(true, "Prepare to face your end."); break;
                    }

                }
            }
        }

        public GreenNinjaBoss(Serial serial) : base(serial)
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
