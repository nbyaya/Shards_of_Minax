using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Paul Bunyan")]
    public class UltimateMasterLumberjack : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterLumberjack()
            : base(AIType.AI_Melee)
        {
            Name = "Paul Bunyan";
            Title = "The Legendary Lumberjack";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(15000);
            SetMana(1000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Lumberjacking, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;

            AddItem(new FancyShirt(Utility.RandomRedHue()));
            AddItem(new LongPants());
            AddItem(new Boots());

            HairItemID = 0x2048; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterLumberjack(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PaulsAxe), typeof(BlueOxTotem) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(PaulsAxe), typeof(BlueOxTotem) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(PaulsAxe) }; }
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

            c.DropItem(new PowerScroll(SkillName.Lumberjacking, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PaulsAxe());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new BlueOxTotem());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: AxeToss(defender); break;
                    case 1: Earthquake(); break;
                    case 2: TimberCall(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void AxeToss(Mobile target)
        {
            if (target != null)
            {
                DoHarmful(target);
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                target.PlaySound(0x207);

                int damage = Utility.RandomMinMax(80, 100);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
            }
        }

        public void Earthquake()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(60, 80);
                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
            }
        }

        public void TimberCall()
        {
            for (int i = 0; i < 3; i++)
            {
                TreeEnt ent = new TreeEnt();
                ent.MoveToWorld(this.Location, this.Map);
                ent.Combatant = this.Combatant;
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

    public class PaulsAxe : Item
    {
        [Constructable]
        public PaulsAxe() : base(0xF49)
        {
            Name = "Paul's Axe";
            Hue = 0x47E;
        }

        public PaulsAxe(Serial serial) : base(serial)
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

    public class BlueOxTotem : Item
    {
        [Constructable]
        public BlueOxTotem() : base(0x2C95)
        {
            Name = "Blue Ox Totem";
            Hue = 0x47E;
        }

        public BlueOxTotem(Serial serial) : base(serial)
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

    public class TreeEnt : BaseCreature
    {
        [Constructable]
        public TreeEnt() : base(AIType.AI_Melee, FightMode.Evil, 10, 1, 0.1, 0.2)
        {
            Name = "Tree Ent";
            Body = 301;
            Hue = 0x47E;

            SetStr(150, 200);
            SetDex(30, 50);
            SetInt(30, 50);

            SetHits(300);
            SetMana(0);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 60.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 30;
        }

        public TreeEnt(Serial serial) : base(serial)
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
