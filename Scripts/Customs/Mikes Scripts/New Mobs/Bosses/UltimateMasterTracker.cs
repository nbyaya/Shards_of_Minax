using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Geronimo")]
    public class UltimateMasterTracker : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterTracker()
            : base(AIType.AI_Melee)
        {
            Name = "Geronimo";
            Title = "The Ultimate Tracker";
            Body = 0x190;
            Hue = 0x83E;

            SetStr(300, 450);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(10000);
            SetMana(1500);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Tracking, 120.0);
            SetSkill(SkillName.Archery, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 60;

            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new LeatherArms());
            AddItem(new LeatherGorget());
            AddItem(new LeatherGloves());
            AddItem(new ThighBoots());

            HairItemID = 0x203C; // Ponytail
            HairHue = 0x47E;
        }

        public UltimateMasterTracker(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(HuntersBow), typeof(HuntersBow) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(HuntersBow), typeof(HuntersBow) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(HuntersBow), typeof(HuntersBow) }; }
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

            c.DropItem(new PowerScroll(SkillName.Tracking, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaxxiaScroll());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaxxiaScroll());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: MarkPrey(defender); break;
                    case 1: HunterStrike(defender); break;
                    case 2: Evasion(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void MarkPrey(Mobile defender)
        {

                int damage = Utility.RandomMinMax(70, 90);

                defender.Damage(damage, this);
                defender.FixedParticles(0x37B9, 10, 20, 5029, EffectLayer.Head);
                defender.PlaySound(0x208);

        }

        public void HunterStrike(Mobile defender)
        {

                int damage = Utility.RandomMinMax(70, 90);

                defender.Damage(damage, this);
                defender.FixedParticles(0x37B9, 10, 20, 5029, EffectLayer.Head);
                defender.PlaySound(0x208);

        }

        public void Evasion()
        {
            this.SendMessage("Geronimo evades your attack!");

            this.VirtualArmorMod += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(10), delegate { this.VirtualArmorMod -= 20; });
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

    public class TrackerBoots : BaseClothing
    {
        [Constructable]
        public TrackerBoots()
            : base(0x170B)
        {
            Name = "Tracker's Boots";
            Hue = 0x497;
            Attributes.BonusDex = 5;
            Attributes.LowerManaCost = 10;
        }

        public TrackerBoots(Serial serial)
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

    public class HuntersBow : Item
    {
        [Constructable]
        public HuntersBow()
            : base(0x13B2)
        {
            Name = "Hunter's Bow";
            Hue = 0x497;
        }

        public HuntersBow(Serial serial)
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
