using System;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sheepdog overlord")]
    public class SheepdogHandlerBoss : SheepdogHandler
    {
        [Constructable]
        public SheepdogHandlerBoss() : base()
        {
            Name = "Sheepdog Overlord";
            Title = "the Supreme Handler";

            // Enhanced stats to match or exceed Barracoon
            SetStr(425); // Upper end strength
            SetDex(150); // Upper end dexterity
            SetInt(750); // Upper end intelligence

            SetHits(12000); // High hit points like a boss
            SetStam(300); // Increased stamina
            SetMana(750); // High mana

            SetDamage(29, 38); // Higher damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach a random ability for extra unpredictability
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
            // Additional boss logic could be added here if needed
        }

        public SheepdogHandlerBoss(Serial serial) : base(serial)
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
