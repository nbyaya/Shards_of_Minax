using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Lucrezia Borgia")]
    public class UltimateMasterPoisoner : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterPoisoner()
            : base(AIType.AI_Mage)
        {
            Name = "Lucrezia Borgia";
            Title = "The Mistress of Poison";
            Body = 0x191;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Poisoning, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
            
            AddItem(new FancyDress(Utility.RandomPinkHue()));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x94;
        }

        public UltimateMasterPoisoner(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PoisonedDagger), typeof(VialOfToxins) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(LethalPoisonPotion), typeof(GreaterPoisonPotion) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(PoisonousPlant), typeof(ToxicCauldron) }; }
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

            c.DropItem(new PowerScroll(SkillName.Poisoning, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PoisonedDagger());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new VialOfToxins());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: VenomStrike(defender); break;
                    case 1: ToxicCloud(); break;
                    case 2: Antidote(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void VenomStrike(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                AOS.Damage(defender, this, Utility.RandomMinMax(60, 80), 0, 0, 0, 100, 0);
                defender.ApplyPoison(this, Poison.Lethal);
                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x207);
            }
        }

        public void ToxicCloud()
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

                m.ApplyPoison(this, Poison.Lethal);

                m.FixedParticles(0x36B0, 1, 14, 0x26B8, 0x3F, 0x7, EffectLayer.Head);
                m.PlaySound(0x229);
            }
        }

        public void Antidote()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                if (m.Poisoned)
                {
                    m.CurePoison(this);
                    m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    m.PlaySound(0x1E0);
                }
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
}
