using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dark elf overlord")]
    public class DarkElfBoss : DarkElf
    {
        [Constructable]
        public DarkElfBoss() : base()
        {
            Name = "Dark Elf Overlord";
            Title = "the Malevolent Supreme";

            // Update stats to match or exceed the original dark elf and increase their power
            SetStr(500, 750); // Strength is higher
            SetDex(500, 750); // Dexterity is higher
            SetInt(500, 750); // Intelligence is higher
            SetHits(9000); // Much higher health than the original

            SetDamage(200, 300); // Increased damage range

            SetSkill(SkillName.Magery, 200.0, 250.0);
            SetSkill(SkillName.Fencing, 200.0, 250.0);
            SetSkill(SkillName.Macing, 200.0, 250.0);
            SetSkill(SkillName.MagicResist, 200.0, 250.0);
            SetSkill(SkillName.Swords, 200.0, 250.0);
            SetSkill(SkillName.Tactics, 200.0, 250.0);
            SetSkill(SkillName.Wrestling, 200.0, 250.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100; // Much higher armor

            // Attach the random ability for extra randomness and challenge
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

            // Increase the loot quality
            AddLoot(LootPack.UltraRich); // Rich loot for a boss
        }

        public override void OnThink()
        {
            base.OnThink();
            // You can add additional boss behavior here, such as special abilities or effects.
        }

        public DarkElfBoss(Serial serial) : base(serial)
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
