using System;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ogre master overlord")]
    public class OgreMasterBoss : OgreMaster
    {
        [Constructable]
        public OgreMasterBoss() : base()
        {
            Name = "Ogre Master Overlord";
            Title = "the Supreme Ogre Master";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Increased strength to make it more powerful
            SetDex(255); // Maximized dexterity
            SetInt(250); // Maximized intelligence

            SetHits(12000); // Increased health to match Barracoon's level

            SetDamage(29, 38); // Increased damage range to make the boss more dangerous

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;

            // Attach the XmlRandomAbility for extra functionality
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic can be added here
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Additional drops, such as gold and items
            PackGold(500, 800);
            PackItem(new Club());
            PackItem(new Robe(Utility.RandomRedHue()));
            PackItem(new Sandals(Utility.RandomRedHue()));
        }

        public OgreMasterBoss(Serial serial) : base(serial)
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
