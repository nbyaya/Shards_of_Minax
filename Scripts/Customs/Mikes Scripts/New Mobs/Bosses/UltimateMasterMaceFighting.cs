using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Thor")]
    public class UltimateMasterMaceFighting : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterMaceFighting()
            : base(AIType.AI_Melee)
        {
            Name = "Thor";
            Title = "The Thunder God";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(500, 650);
            SetDex(100, 150);
            SetInt(100, 150);

            SetHits(15000);
            SetMana(1000);

            SetDamage(40, 50);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Macing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Parry, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;

            AddItem(new NorseHelm());
            AddItem(new ChainChest());
            AddItem(new ChainLegs());
            AddItem(new PlateArms());
            AddItem(new PlateGorget());
            AddItem(new PlateGloves());
            AddItem(new Boots());

            HairItemID = 0x204B; // Long Hair
            HairHue = 0x47E;


        }

        public UltimateMasterMaceFighting(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(Mjolnir), typeof(ThunderGodsBelt) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ThorHammer), typeof(ThunderGodsShield) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ThorStatue) }; }
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

            c.DropItem(new PowerScroll(SkillName.Macing, 200.0));

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
                    case 0: ThunderStrike(defender); break;
                    case 1: HammerSpin(); break;
                    case 2: GodlyStrength(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void ThunderStrike(Mobile defender)
        {
            if (defender != null)
            {
                defender.BoltEffect(0);
                int damage = Utility.RandomMinMax(80, 100);
                AOS.Damage(defender, this, damage, 0, 0, 0, 100, 0);
                defender.SendMessage("You are struck by Thor's thunderous might!");
            }
        }

        public void HammerSpin()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);
                int damage = Utility.RandomMinMax(50, 70);
                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                m.Freeze(TimeSpan.FromSeconds(2.0));
                m.SendMessage("You are stunned by Thor's hammer spin!");
            }
        }

        public void GodlyStrength()
        {
            this.SendMessage("Thor's godly strength surges through him!");
            this.Hits += 1000;
            this.DamageMin += 20;
            this.DamageMax += 20;
            this.PlaySound(0x1F7);
            this.FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist);
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

    public class Mjolnir : Item
    {
        [Constructable]
        public Mjolnir()
            : base(0xE81)
        {
            Name = "Mjolnir";
            Hue = 0x482;
            Weight = 8.0;
        }

        public Mjolnir(Serial serial) : base(serial)
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

    public class ThunderGodsBelt : Item
    {
        [Constructable]
        public ThunderGodsBelt()
            : base(0x2B68)
        {
            Name = "Thunder God's Belt";
            Hue = 0x482;
            Weight = 1.0;
        }

        public ThunderGodsBelt(Serial serial) : base(serial)
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
