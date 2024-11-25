using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master cabinet maker")]
    public class CabinetMakerBoss : CabinetMaker
    {
        [Constructable]
        public CabinetMakerBoss() : base()
        {
            Name = "Master Cabinet Maker";
            Title = "the Grand Creator";

            // Enhance stats to match or exceed those of a boss-tier creature
            SetStr(700, 900); // Enhanced strength
            SetDex(250, 300); // Enhanced dexterity
            SetInt(400, 600); // Enhanced intelligence

            SetHits(1500, 2000); // Enhanced health for a boss

            SetDamage(15, 30); // Increased damage for a boss-tier NPC

            // Enhanced resistance values
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Carpentry, 120.0, 150.0); // Increased crafting skills
            SetSkill(SkillName.Tinkering, 110.0, 140.0); 
            SetSkill(SkillName.Blacksmith, 100.0, 130.0);
            SetSkill(SkillName.Lumberjacking, 90.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 15000; // Increased fame for a boss
            Karma = -15000; // Increased karma penalty for a boss

            VirtualArmor = 60; // Enhanced armor value for a boss

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new CougarSummoningMateria());
			PackItem(new PsychedelicTieDyeShirt());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
            
            // Additional loot specific to the boss
            PackGold(500, 700); // Increased gold drop
            AddLoot(LootPack.Average); // Standard loot pack for a boss

            // Say phrases like a boss
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "You dare challenge my craftsmanship?"); break;
                case 1: this.Say(true, "My creations will destroy you!"); break;
            }

            // Drop items specific to the Cabinet Maker theme
            PackItem(new Board(Utility.RandomMinMax(30, 50)));
            PackItem(new Hammer(Utility.RandomNeutralHue()));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss-specific behavior can be added here
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say(true, "My work... unfinished..."); break;
                    case 1: this.Say(true, "My tools... they'll find another..."); break;
                    case 2: this.Say(true, "No one shall defeat my craftsmanship!"); break;
                    case 3: this.Say(true, "You can't destroy my creations!"); break;
                }
            }
        }

        public CabinetMakerBoss(Serial serial) : base(serial)
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
