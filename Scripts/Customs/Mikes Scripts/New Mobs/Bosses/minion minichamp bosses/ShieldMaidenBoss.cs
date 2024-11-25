using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the shield overlord")]
    public class ShieldMaidenBoss : ShieldMaiden
    {
        [Constructable]
        public ShieldMaidenBoss() : base()
        {
            Name = "Shield Overlord";
            Title = "the Unbreakable";

            // Update stats to match or exceed Barracoon and other bosses
            SetStr(1200); // Higher strength than the original
            SetDex(80); // Higher dexterity
            SetInt(60); // Higher intelligence

            SetHits(12000); // Much higher health to make it a boss
            SetDamage(30, 45); // Increased damage range for a tougher fight

            // Resistances: Increase physical and elemental resistances
            SetResistance(ResistanceType.Physical, 85, 95);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Skills: Set high skills for a challenging boss
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Parry, 120.0, 140.0);

            Fame = 25000; // Higher fame to represent the boss status
            Karma = -25000; // Negative karma to mark it as a villain

            VirtualArmor = 100; // Higher virtual armor to make it tougher

            // Attach a random ability for additional challenge
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

            // Boss-specific loot dialogue
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "You are worthy, but not for long!"); break;
                case 1: this.Say(true, "My shield shall protect me... forever!"); break;
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Add any extra behavior or abilities for the boss if needed
        }

        public ShieldMaidenBoss(Serial serial) : base(serial)
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
