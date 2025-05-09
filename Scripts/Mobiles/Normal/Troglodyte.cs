using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a troglodyte corpse")]
    public class Troglodyte : BaseCreature
    {
        public override double HealChance { get { return 1.0; } }

        [Constructable]
        public Troglodyte()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)// NEED TO CHECK
        {
            Name = "a troglodyte";
            Body = 267;
            BaseSoundID = 0x59F; 

            SetStr(148, 217);
            SetDex(91, 120);
            SetInt(51, 70);

            SetHits(302, 340);

            SetDamage(11, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 35, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 70.5, 94.8);
            SetSkill(SkillName.MagicResist, 51.8, 65.0);
            SetSkill(SkillName.Tactics, 80.4, 94.7);
            SetSkill(SkillName.Wrestling, 70.2, 93.5);
            SetSkill(SkillName.Healing, 70.0, 95.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 28; // Don't know what it should be

            PackItem(new Bandage(5));  // How many?
            PackItem(new Ribs());

            for (int i = 0; i < Utility.RandomMinMax(0, 1); i++)
            {
                PackItem(Loot.RandomScroll(0, Loot.ArcanistScrollTypes.Length, SpellbookType.Arcanist));
            }
        }

        public Troglodyte(Serial serial)
            : base(serial)
        {
        }
		
		public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);  // Need to verify
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new TroglodyteSummonerKryss());
            }			
        }
		
		public override void OnDeath( Container c )
        {
        base.OnDeath( c );

            if ( Utility.RandomDouble() < 0.1 )
            c.DropItem( new PrimitiveFetish() );
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}