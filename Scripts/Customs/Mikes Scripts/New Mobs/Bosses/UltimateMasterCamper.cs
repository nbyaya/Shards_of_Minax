using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Bear Grylls")]
    public class UltimateMasterCamper : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterCamper()
            : base(AIType.AI_Melee)
        {
            Name = "Bear Grylls";
            Title = "The Survival Expert";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(400, 500);
            SetDex(200, 250);
            SetInt(200, 250);

            SetHits(15000);
            SetMana(1000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Camping, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Healing, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;
			
			AddItem(new Boots());
            AddItem(new BearMask());
            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new LeatherGloves());
            AddItem(new LeatherArms());

            HairItemID = 0x2048; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterCamper(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(SurvivalistsTent), typeof(FirestarterKit) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(BearGryllsJournal), typeof(SurvivalKnife) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(SurvivalistsTent), typeof(SurvivalBackpack) }; }
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

            c.DropItem(new PowerScroll(SkillName.Camping, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new SurvivalistsTent());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new FirestarterKit());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Bushcraft(); break;
                    case 1: SurvivalInstinct(); break;
                    case 2: FireMastery(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Bushcraft()
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

                m.Paralyze(TimeSpan.FromSeconds(5.0));

                m.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                m.PlaySound(0x1FA);
            }
        }

        public void SurvivalInstinct()
        {
            Hits += 1000;

            FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            PlaySound(0x1F2);
        }

        public void FireMastery()
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

                int damage = Utility.RandomMinMax(80, 100);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                m.PlaySound(0x208);
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

    public class SurvivalistsTent : Item
    {
        [Constructable]
        public SurvivalistsTent()
            : base(0xE58)
        {
            Name = "Survivalist's Tent";
            Hue = 0x47E;
        }

        public SurvivalistsTent(Serial serial)
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

    public class FirestarterKit : Item
    {
        [Constructable]
        public FirestarterKit()
            : base(0xF0C)
        {
            Name = "Firestarter Kit";
            Hue = 0x47E;
        }

        public FirestarterKit(Serial serial)
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
