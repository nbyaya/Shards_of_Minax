using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the BattleWeaver Overlord")]
    public class BattleWeaverBoss : BattleWeaver
    {
        [Constructable]
        public BattleWeaverBoss() : base()
        {
            Name = "BattleWeaver Overlord";
            Title = "the Supreme Weaver";

            // Update stats to match or exceed Barracoon
            SetStr(700); // Upper bound from Barracoon or better
            SetDex(250); // Matching the upper end of BattleWeaver
            SetInt(600); // Upper bound from Barracoon or better

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Enhanced damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 60, 75); // Higher resistance
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 70.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;

            // Attach a random ability
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

            // Custom messages
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "The threads... of fate... are mine to control!"); break;
                case 1: this.Say(true, "You dare challenge the weaver of destiny?!"); break;
            }
        }

        public BattleWeaverBoss(Serial serial) : base(serial)
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
