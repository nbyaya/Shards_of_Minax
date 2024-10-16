using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Gandalf")]
    public class UltimateMasterMagicResistance : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterMagicResistance()
            : base(AIType.AI_Mage)
        {
            Name = "Gandalf";
            Title = "The Grey Wizard";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(400, 500);
            SetDex(100, 150);
            SetInt(700, 900);

            SetHits(15000);
            SetMana(3000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = 24000;

            VirtualArmor = 80;

            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterMagicResistance(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(WizardsStaff), typeof(MithrilArmor) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(Spellbook), typeof(MagicalWand) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ArcaneCrystal), typeof(EnchantedLantern) }; }
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

            c.DropItem(new PowerScroll(SkillName.MagicResist, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new WizardsStaff());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MithrilArmor());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: ShieldOfLight(); break;
                    case 1: DispelMagic(); break;
                    case 2: ResistMagic(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void ShieldOfLight()
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

                int damage = Utility.RandomMinMax(40, 60);

                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                m.PlaySound(0x1F7);
            }
        }

        public void DispelMagic()
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

                m.Paralyzed = false;
                m.Poison = null;

                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                m.PlaySound(0x1F1);
            }
        }

        public void ResistMagic()
        {
            this.Say("You shall not pass!");

            this.FixedParticles(0x373A, 10, 30, 5036, EffectLayer.Waist);
            this.PlaySound(0x1EA);

            this.Hits += 1000; // Heal 1000 HP
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

    public class WizardsStaff : Item
    {
        [Constructable]
        public WizardsStaff() : base(0xDF2)
        {
            Weight = 5.0;
            Name = "Wizard's Staff";
            Hue = 0x482;
        }

        public WizardsStaff(Serial serial) : base(serial)
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

    public class MithrilArmor : Item
    {
        [Constructable]
        public MithrilArmor() : base(0x13BE)
        {
            Weight = 10.0;
            Name = "Mithril Armor";
            Hue = 0x835;
        }

        public MithrilArmor(Serial serial) : base(serial)
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
