using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the earth clan ninja overlord")]
    public class EarthClanNinjaBoss : EarthClanNinja
    {
        [Constructable]
        public EarthClanNinjaBoss() : base()
        {
            Name = "Earth Clan Ninja Overlord";
            Title = "the Mountain's Wrath";

            // Update stats to match or exceed EarthClanNinja
            SetStr(900); // Matching upper range of strength
            SetDex(450); // Matching upper range of dexterity
            SetInt(250); // Matching upper range of intelligence

            SetHits(1600); // Matching or exceeding the upper health of EarthClanNinja

            SetDamage(15, 25); // Enhancing damage range

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 65);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.MagicResist, 200.0);
            SetSkill(SkillName.Tactics, 200.0);
            SetSkill(SkillName.Wrestling, 150.0);
            SetSkill(SkillName.Ninjitsu, 120.0); // Increasing Ninjitsu skill

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 90;

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
        }

        public EarthClanNinjaBoss(Serial serial) : base(serial)
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
