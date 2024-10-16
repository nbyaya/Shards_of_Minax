using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Autolycus")]
    public class UltimateMasterThief : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterThief()
            : base(AIType.AI_Melee)
        {
            Name = "Autolycus";
            Title = "The King of Thieves";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(505, 750);
            SetInt(72, 150);

            SetHits(12000);
            SetMana(1000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new LeatherArms());
            AddItem(new LeatherGloves());
            AddItem(new Boots());
            AddItem(new Cloak());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x47E;
        }

        public UltimateMasterThief(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ThiefsHood), typeof(PurseOfTheRich) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ThiefsTools), typeof(LockpickSet) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(GoldenChalice), typeof(SilverGoblet) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Stealing, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ThiefsHood());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PurseOfTheRich());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Pickpocket(defender); break;
                    case 1: QuickEscape(); break;
                    case 2: ShadowStrike(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Pickpocket(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                target.SendMessage("You feel something being stolen from you!");
                // Logic for stealing items or gold
                target.Hits -= Utility.RandomMinMax(10, 20);
                this.Hits += Utility.RandomMinMax(10, 20);
                this.Mana += Utility.RandomMinMax(10, 20);
            }
        }

        public void QuickEscape()
        {
            this.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            this.PlaySound(0x1FE);
            this.Hidden = true;
            // Logic for increased speed and evasion
        }

        public void ShadowStrike(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                target.SendMessage("You are struck from the shadows!");
                target.Hits -= Utility.RandomMinMax(60, 80);
                this.Hits += Utility.RandomMinMax(60, 80);
                this.Mana += Utility.RandomMinMax(60, 80);
            }
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


	public class ThiefsHood : BaseHat
	{
		public override int BasePhysicalResistance { get { return 3; } }
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 2; } }

		[Constructable]
		public ThiefsHood()
			: base(0x1540)
		{
			Weight = 1.0;
			Name = "Thief's Hood";
			Hue = 0x455;
			Attributes.NightSight = 1;
			Attributes.Luck = 25;
		}

		public ThiefsHood(Serial serial)
			: base(serial)
		{
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

	public class PurseOfTheRich : Item
	{
		[Constructable]
		public PurseOfTheRich()
			: base(0xE76)
		{
			Weight = 1.0;
			Name = "Purse of the Rich";
			Hue = 0x501;
		}

		public PurseOfTheRich(Serial serial)
			: base(serial)
		{
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

	public class ThiefsTools : Item
	{
		[Constructable]
		public ThiefsTools()
			: base(0x1EB8)
		{
			Weight = 1.0;
			Name = "Thief's Tools";
			Hue = 0x973;
		}

		public ThiefsTools(Serial serial)
			: base(serial)
		{
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

	public class GoldenChalice : Item
	{
		[Constructable]
		public GoldenChalice()
			: base(0xE2B)
		{
			Weight = 1.0;
			Name = "Golden Chalice";
			Hue = 0x8A5;
		}

		public GoldenChalice(Serial serial)
			: base(serial)
		{
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

	public class SilverGoblet : Item
	{
		[Constructable]
		public SilverGoblet()
			: base(0xE2C)
		{
			Weight = 1.0;
			Name = "Silver Goblet";
			Hue = 0x8A5;
		}

		public SilverGoblet(Serial serial)
			: base(serial)
		{
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
