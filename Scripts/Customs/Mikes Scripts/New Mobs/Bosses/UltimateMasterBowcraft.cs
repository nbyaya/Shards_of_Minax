using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of William Tell")]
    public class UltimateMasterBowcraft : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterBowcraft()
            : base(AIType.AI_Archer)
        {
            Name = "William Tell";
            Title = "The Master Archer";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(372, 520);
            SetInt(285, 375);

            SetHits(15000);
            SetMana(2000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Archery, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Fletching, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 70;

            AddItem(new FancyShirt(Utility.RandomGreenHue()));
            AddItem(new LongPants(Utility.RandomGreenHue()));
            AddItem(new Boots(Utility.RandomGreenHue()));
            AddItem(new Cloak(Utility.RandomRedHue()));
            AddItem(new CompositeBow());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x457;

            FacialHairItemID = 0x204B; // Mustache
            FacialHairHue = 0x457;
        }

        public UltimateMasterBowcraft(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(CrossbowOfPrecision), typeof(TellsApple) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ArrowOfPiercing), typeof(BlackQuiver) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(CrossbowOfPrecision), typeof(TellsApple) }; }
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

            c.DropItem(new PowerScroll(SkillName.Fletching, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new CrossbowOfPrecision());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new TellsApple());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: AppleShot(defender); break;
                    case 1: RapidReload(); break;
                    case 2: PrecisionAim(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void AppleShot(Mobile defender)
        {
            if (defender != null)
            {
                this.MovingParticles(defender, 0x36E4, 5, 0, false, true, 0x23, 0, 9502, 4019, 0x160, 0);
                AOS.Damage(defender, this, Utility.RandomMinMax(50, 80), 100, 0, 0, 0, 0);
                defender.Freeze(TimeSpan.FromSeconds(3));
                defender.SendLocalizedMessage(1070840); // You have been stunned!
            }
        }

        public void RapidReload()
        {
            this.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
            this.PlaySound(0x1E3);
            this.SendLocalizedMessage(1070845); // You reload at lightning speed!
            
            double skillValue = this.Skills[SkillName.Archery].Value;
            TimeSpan delay = TimeSpan.FromSeconds(2.0 - (skillValue / 200));
            new RapidReloadTimer(this, delay).Start();
        }

        public void PrecisionAim(Mobile defender)
        {
            if (defender != null)
            {
                this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                this.PlaySound(0x1F2);
                defender.FixedParticles(0x37C4, 1, 31, 9963, 0, 0, EffectLayer.Head);

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(DoPrecisionHit), defender);
            }
        }

        private void DoPrecisionHit(object state)
        {
            if (state is Mobile)
            {
                Mobile defender = (Mobile)state;
                AOS.Damage(defender, this, Utility.RandomMinMax(60, 80), 100, 0, 0, 0, 0);
                defender.SendLocalizedMessage(1070846); // You've been hit by a precision shot!
            }
        }

        private class RapidReloadTimer : Timer
        {
            private Mobile m_Mobile;

            public RapidReloadTimer(Mobile m, TimeSpan delay) : base(delay, delay)
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && !m_Mobile.Deleted)
                {
                    m_Mobile.SendLocalizedMessage(1070849); // Your rapid reload effect has worn off.
                }

                this.Stop();
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