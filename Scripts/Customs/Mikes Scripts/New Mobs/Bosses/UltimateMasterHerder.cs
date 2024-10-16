using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Little Bo Peep")]
    public class UltimateMasterHerder : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterHerder()
            : base(AIType.AI_Melee)
        {
            Name = "Little Bo Peep";
            Title = "The Shepherdess";
            Body = 0x191;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.AnimalTaming, 120.0);
            SetSkill(SkillName.Herding, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 70;

            AddItem(new FancyShirt(Utility.RandomYellowHue()));
            AddItem(new LongPants(Utility.RandomBlueHue()));
            AddItem(new Cloak(Utility.RandomPinkHue()));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterHerder(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ShepherdsCrook), typeof(WoolenCloak) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(HerdingTome), typeof(Fleece) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(WoolenCloak), typeof(SheepStatuette) }; }
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

            c.DropItem(new PowerScroll(SkillName.Herding, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ShepherdsCrook());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new WoolenCloak());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: CallOfTheFlock(); break;
                    case 1: ProtectiveHerd(); break;
                    case 2: ShepherdsBlessing(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void CallOfTheFlock()
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

                AOS.Damage(m, this, damage, 0, 0, 0, 50, 50);

                m.FixedParticles(0x3728, 10, 15, 5042, EffectLayer.Head);
                m.PlaySound(0x213);
            }
        }

        public void ProtectiveHerd()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                m.PlaySound(0x1F2);

                m.SendLocalizedMessage(1070841); // You feel the protection of the herd!

                m.AddStatMod(new StatMod(StatType.Dex, "ProtectiveHerd", 10, TimeSpan.FromSeconds(30.0)));
                m.AddStatMod(new StatMod(StatType.Str, "ProtectiveHerd", 10, TimeSpan.FromSeconds(30.0)));
            }
        }

        public void ShepherdsBlessing()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head);
                m.PlaySound(0x1F5);

                m.SendLocalizedMessage(1070842); // You feel blessed by the shepherd!

                m.AddStatMod(new StatMod(StatType.Dex, "ShepherdsBlessing", 15, TimeSpan.FromSeconds(45.0)));
                m.AddStatMod(new StatMod(StatType.Str, "ShepherdsBlessing", 15, TimeSpan.FromSeconds(45.0)));
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
