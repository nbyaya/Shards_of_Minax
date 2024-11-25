using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the BattleHerbalist Overlord")]
    public class BattleHerbalistBoss : BattleHerbalist
    {
        [Constructable]
        public BattleHerbalistBoss() : base()
        {
            Name = "BattleHerbalist Overlord";
            Title = "the Supreme Healer";

            // Update stats to match or exceed Barracoon-style boss NPCs
            SetStr(700); // Enhanced Strength
            SetDex(200); // Enhanced Dexterity
            SetInt(400); // Enhanced Intelligence

            SetHits(12000); // Enhanced Health
            SetDamage(25, 35); // Enhanced Damage

            SetResistance(ResistanceType.Physical, 70, 90); // Enhanced Resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 100.0); // Enhanced Magic Resistance
            SetSkill(SkillName.Tactics, 100.0); // Enhanced Tactics
            SetSkill(SkillName.Wrestling, 100.0); // Enhanced Wrestling

            Fame = 10000; // Increased Fame to reflect boss status
            Karma = 10000; // Increased Karma to reflect boss status

            VirtualArmor = 70; // Enhanced Virtual Armor

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
            // Additional boss logic could be added here, e.g., special abilities or behaviors
        }

        public BattleHerbalistBoss(Serial serial) : base(serial)
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
