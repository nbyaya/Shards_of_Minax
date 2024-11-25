using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the map overlord")]
    public class EvilMapMakerBoss : EvilMapMaker
    {
        [Constructable]
        public EvilMapMakerBoss() : base()
        {
            Name = "Map Overlord";
            Title = "the Supreme Map Maker";

            // Update stats to match or exceed Barracoon's levels
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(200); // Enhanced dexterity for the boss
            SetInt(750); // High intelligence for powerful magic

            SetHits(12000); // High health, like Barracoon
            SetStam(300); // High stamina
            SetMana(750); // High mana pool

            SetDamage(29, 38); // Damage range similar to Barracoon's

            SetResistance(ResistanceType.Physical, 70, 80); // Stronger resistances
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 80.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 75; // Stronger armor

            // Attach the random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new ZombieHand());
			PackItem(new IlluminaDagger());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic can be added here (e.g., special abilities or behaviors)
        }

        public EvilMapMakerBoss(Serial serial) : base(serial)
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
