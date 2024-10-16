using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of MacGyver")]
    public class UltimateMasterRemoveTrap : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterRemoveTrap()
            : base(AIType.AI_Melee)
        {
            Name = "MacGyver";
            Title = "The Ultimate Escape Artist";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(200, 250);

            SetHits(15000);
            SetMana(1000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.RemoveTrap, 120.0);
            SetSkill(SkillName.Lockpicking, 120.0);
            SetSkill(SkillName.DetectHidden, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 80;

            AddItem(new FancyShirt(Utility.RandomBlueHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Boots());
            AddItem(new Bandana(Utility.RandomRedHue()));

            HairItemID = 0x203C; // Medium Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterRemoveTrap(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(MultiTool), typeof(ImprovisedBomb) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(MacGyverToolkit), typeof(LockpickingKit) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(TrapDisarmKit), typeof(TinkerTools) }; }
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

            c.DropItem(new PowerScroll(SkillName.RemoveTrap, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MultiTool());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ImprovisedBomb());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Disarm(); break;
                    case 1: ExplosiveExit(); break;
                    case 2: TrapExpert(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Disarm()
        {
            this.AddStatMod(new StatMod(StatType.Dex, "TrapExpertDex", 50, TimeSpan.FromMinutes(2)));

            this.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            this.PlaySound(0x20F);
        }

        public void ExplosiveExit()
        {
            this.AddStatMod(new StatMod(StatType.Dex, "TrapExpertDex", 50, TimeSpan.FromMinutes(2)));

            this.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            this.PlaySound(0x20F);
        }

        public void TrapExpert()
        {
            this.AddStatMod(new StatMod(StatType.Dex, "TrapExpertDex", 50, TimeSpan.FromMinutes(2)));

            this.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            this.PlaySound(0x20F);
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


	public class MultiTool : Item
	{
		[Constructable]
		public MultiTool()
			: base(0x1EBA)
		{
			Weight = 1.0;
			Name = "Multi-tool";
			Hue = 0x482;
		}

		public MultiTool(Serial serial)
			: base(serial)
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

	public class ImprovisedBomb : Item
	{
		[Constructable]
		public ImprovisedBomb()
			: base(0x1EA7)
		{
			Weight = 2.0;
			Name = "Improvised Bomb";
			Hue = 0x455;
		}

		public ImprovisedBomb(Serial serial)
			: base(serial)
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