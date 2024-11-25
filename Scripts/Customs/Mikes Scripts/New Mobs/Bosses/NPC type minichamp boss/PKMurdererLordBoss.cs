using System;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the PKMurderer Lord")]
    public class PKMurdererLordBoss : PKMurdererLord
    {
        [Constructable]
        public PKMurdererLordBoss() : base()
        {
            Name = "PKMurderer Lord";
            Title = "the Supreme Executioner";

            // Enhance stats to match or exceed Barracoon's power
            SetStr(1200); // Match or exceed Barracoon's upper strength
            SetDex(255);  // Match the highest dexterity of Barracoon
            SetInt(250);  // Use the upper bound of the original intelligence stat

            SetHits(12000); // Higher health
            SetDamage(29, 38); // Match Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 100);  // Keep the original resistance
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 150.0);  // Enhance resistance skills
            SetSkill(SkillName.Tactics, 120.0);  // Boost tactics skill for more damage
            SetSkill(SkillName.Wrestling, 120.0);  // Boost wrestling skill

            Fame = 22500;  // Match Barracoon's fame
            Karma = -22500;  // Match Barracoon's karma

            VirtualArmor = 70;  // Match Barracoon's virtual armor

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

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public PKMurdererLordBoss(Serial serial) : base(serial)
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
