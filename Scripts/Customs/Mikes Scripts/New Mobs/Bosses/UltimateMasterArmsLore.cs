using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Miyamoto Musashi")]
    public class UltimateMasterArmsLore : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterArmsLore()
            : base(AIType.AI_Melee)
        {
            Name = "Miyamoto Musashi";
            Title = "The Sword Saint";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(405, 525);
            SetDex(172, 250);
            SetInt(305, 450);

            SetHits(15000);
            SetMana(2000);

            SetDamage(35, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.ArmsLore, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Macing, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Parry, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 80;

            AddItem(new Katana());
            AddItem(new Wakizashi());
            AddItem(new LeatherDo());
            AddItem(new LeatherHiroSode());
            AddItem(new LeatherSuneate());
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x455;
        }

        public UltimateMasterArmsLore(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(SwordOfTheFiveRings), typeof(ManualOfMartialArts) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(BookOfBushido), typeof(SamuraiHelm) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(SwordOfTheFiveRings), typeof(ManualOfMartialArts) }; }
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

            c.DropItem(new PowerScroll(SkillName.ArmsLore, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new SwordOfTheFiveRings());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ManualOfMartialArts());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: PerfectStrike(defender); break;
                    case 1: WeaponMastery(); break;
                    case 2: BladeDance(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void PerfectStrike(Mobile defender)
        {
            if (defender != null)
            {
                int damage = 50;
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);
                defender.FixedParticles(0x3779, 1, 30, 9963, 13, 3, EffectLayer.Head);
                defender.PlaySound(0x1E1);
            }
        }

        public void WeaponMastery()
        {
            SkillMod mod = new TimedSkillMod(SkillName.Swords, true, 20.0, TimeSpan.FromSeconds(30));
            AddSkillMod(mod);
            FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
            PlaySound(0x1E3);
        }

        public void BladeDance()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(30, 40);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x374A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
                m.PlaySound(0x1E1);
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

    public class SwordOfTheFiveRings : Katana
    {
        [Constructable]
        public SwordOfTheFiveRings()
            : base()
        {
            Name = "Sword of the Five Rings";
            Hue = 0x489;
            WeaponAttributes.HitLeechHits = 50;
            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 30;
            Attributes.AttackChance = 15;
        }

        public SwordOfTheFiveRings(Serial serial)
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

    public class ManualOfMartialArts : Item
    {
        [Constructable]
        public ManualOfMartialArts()
            : base(0xFBE)
        {
            Name = "Manual of Martial Arts";
            Hue = 0x489;
        }

        public ManualOfMartialArts(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("You read the manual and feel your combat skills improve!");
            from.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, 10.0, TimeSpan.FromHours(1)));
            from.AddSkillMod(new TimedSkillMod(SkillName.Swords, true, 10.0, TimeSpan.FromHours(1)));
            from.AddSkillMod(new TimedSkillMod(SkillName.Fencing, true, 10.0, TimeSpan.FromHours(1)));
            from.AddSkillMod(new TimedSkillMod(SkillName.Macing, true, 10.0, TimeSpan.FromHours(1)));
            Delete();
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