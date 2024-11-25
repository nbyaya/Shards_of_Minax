using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dark sorcerer lord")]
    public class DarkSorcererBoss : DarkSorcerer
    {
        [Constructable]
        public DarkSorcererBoss() : base()
        {
            Name = "Dark Sorcerer Lord";
            Title = "the Supreme Sorcerer";

            // Update stats to match or exceed Barracoon's or other powerful bosses
            SetStr(425); // Upper strength, matching or surpassing Barracoon
            SetDex(150); // Upper dexterity, matching or surpassing Barracoon
            SetInt(750); // Upper intelligence, matching or surpassing Barracoon

            SetHits(12000); // Matching Barracoon's health or higher
            SetStam(300); // Matching Barracoon's stamina or higher
            SetMana(750); // Matching Barracoon's mana or higher

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 120.0); // Enhanced skill levels
            SetSkill(SkillName.Magery, 120.0); // Enhanced skill levels
            SetSkill(SkillName.Meditation, 100.0); // Enhanced skill levels
            SetSkill(SkillName.MagicResist, 150.0); // Enhanced skill levels
            SetSkill(SkillName.Tactics, 100.0); // Enhanced skill levels
            SetSkill(SkillName.Wrestling, 100.0); // Enhanced skill levels

            Fame = 22500; // Higher fame value for a boss
            Karma = -22500; // Negative karma for a villainous boss

            VirtualArmor = 70;

            // Attach a random ability for additional challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new DemonPlatter());
			PackItem(new NaturalistsCloak());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic (e.g., unique speech, AI adjustments) can be added here
        }

        public DarkSorcererBoss(Serial serial) : base(serial)
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
