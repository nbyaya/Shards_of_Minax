using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the pastry overlord")]
    public class PastryChefBoss : PastryChef
    {
        [Constructable]
        public PastryChefBoss() : base()
        {
            Name = "Pastry Overlord";
            Title = "the Supreme Baker";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(800); // Matching Barracoon's upper strength
            SetDex(150); // Maintaining high dexterity
            SetInt(200); // Maximizing intelligence

            SetHits(12000); // Boss-tier health, much higher than original
            SetStam(300); // Matching Barracoon's upper stamina
            SetMana(750); // Matching Barracoon's upper mana

            SetDamage(29, 38); // Enhanced damage to match boss tier

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach the XmlRandomAbility for extra randomness
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

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if desired
        }

        public PastryChefBoss(Serial serial) : base(serial)
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
