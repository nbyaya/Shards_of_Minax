using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the alchemical overlord")]
    public class ChemistBoss : Chemist
    {
        [Constructable]
        public ChemistBoss() : base()
        {
            Name = "Alchemical Overlord";
            Title = "the Supreme Chemist";

            // Update stats to match or exceed Barracoon's (or better if applicable)
            SetStr(500); // Upper range for strength
            SetDex(250); // Upper range for dexterity
            SetInt(600); // Upper range for intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Increased stamina
            SetMana(750); // Matching Barracoon's mana

            SetDamage(25, 35); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 70.0);

            Fame = 22500; // Matching Barracoon's fame
            Karma = -22500; // Matching Barracoon's karma

            VirtualArmor = 70;

            // Attach a random ability for dynamic gameplay
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new LifeLeechAugmentCrystal());
			PackItem(new BladesLineMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Enhanced loot drop
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "My alchemical prowess will not be bested!"); break;
                case 1: this.Say(true, "You have no idea what you've just unleashed..."); break;
            }

            // Extra item related to chemist theme
            PackItem(new Bottle(Utility.RandomMinMax(20, 30)));
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here
        }

        public ChemistBoss(Serial serial) : base(serial)
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
