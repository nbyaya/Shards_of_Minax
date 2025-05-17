using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cultist corpse")]
    public class CultBrigand : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextVenomTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextLeapTime;

        // Dark purple hue for cultists
        private const int UniqueHue = 1175;

        [Constructable]
        public CultBrigand()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Race = Race.Human;
            Hue  = UniqueHue;
            Title = "the cult brigand";

            // Random gender/body/name
            if (this.Female = Utility.RandomBool())
            {
                Body = 401;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 400;
                Name = NameList.RandomName("male");
            }

            BaseSoundID = 0x3F3; // human male/female sound set

            // Stats
            SetStr(300, 350);
            SetDex(120, 140);
            SetInt(80, 100);

            SetHits(1000, 1200);
            SetStam(120, 140);
            SetMana(200, 250);

            // Melee damage
            SetDamage(30, 40);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire,     20, 30);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Energy,   20, 30);
            SetResistance(ResistanceType.Poison,   60, 70);

            // Skills
            SetSkill(SkillName.Tactics,    100.0, 120.0);
            SetSkill(SkillName.Wrestling,   90.0, 100.0);
            SetSkill(SkillName.Poisoning,  100.0, 120.0);
            SetSkill(SkillName.MagicResist, 80.0,  90.0);
            SetSkill(SkillName.Anatomy,     90.0, 100.0);

            Fame       = 18000;
            Karma      = -18000;
            VirtualArmor = 60;
            ControlSlots = 4;

            // Outfit
            AddItem(new Robe(Utility.RandomRedHue()));
            AddItem(new Sandals());
            AddItem(Loot.RandomWeapon());

            // Cooldowns
            m_NextVenomTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            m_NextLeapTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            // Loot
            PackGold(200, 400);
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Ritual of Venom: poison tile burst around self
            if (DateTime.UtcNow >= m_NextVenomTime)
            {
                RitualOfVenom();
                m_NextVenomTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Summon Acolytes: spawn lesser brigands
            else if (DateTime.UtcNow >= m_NextSummonTime)
            {
                SummonAcolytes();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 55));
            }
            // Fanatical Leap: close distance and stun
            else if (DateTime.UtcNow >= m_NextLeapTime && InRange(Combatant.Location, 12))
            {
                FanaticalLeap();
                m_NextLeapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
        }

        // Aura of Fanaticism: chance to confuse or poison on movement
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || Map != m.Map || !InRange(m, 2) || !CanBeHarmful(m, false))
                return;

            if (m is Mobile target && Utility.RandomDouble() < 0.15)
            {
                DoHarmful(target);
                // 50% chance confuse, 50% poison
                if (Utility.RandomBool())
                {
                    target.SendMessage("You reel under the cult's madness!");
                    target.PlaySound(0x212);
                    target.Paralyze(TimeSpan.FromSeconds(3));
                }
                else
                {
                    target.SendMessage("You are seared by venomous rites!");
                    target.ApplyPoison(this, Poison.Lethal);
                }
            }
        }

        // Poisonous burst around self
        private void RitualOfVenom()
        {
            Say("*By the serpents’ gift!*");
            PlaySound(0x226);
            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    PoisonTile pt = new PoisonTile();
                    pt.Hue = UniqueHue;
                    pt.MoveToWorld(loc, Map);
                }
            }
        }

        // Summon 1–3 HumanBrigand acolytes around self
        private void SummonAcolytes()
        {
            Say("*Arise, my disciples!*");
            PlaySound(0x1F9);
            int count = Utility.RandomMinMax(1, 3);
            for (int i = 0; i < count; i++)
            {
                Point3D loc;
                do
                {
                    int dx = Utility.RandomMinMax(-1, 1);
                    int dy = Utility.RandomMinMax(-1, 1);
                    loc = new Point3D(X + dx, Y + dy, Z);
                }
                while (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false));

                var acolyte = new Brigand();
                acolyte.MoveToWorld(loc, Map);
                acolyte.Combatant = this.Combatant; // immediately join fight
            }
        }

        // Leap to target, damage and stun in small AoE
        private void FanaticalLeap()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Feel the wrath!*");
            PlaySound(0x2F1);

            // Visual effect at origin
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)),
                0x3728, 8, 20, UniqueHue, 0, 5044, 0);

            // Teleport near target
            Point3D dest = target.Location;
            MoveToWorld(dest, Map);

            // AoE slam
            foreach (var m in Map.GetMobilesInRange(Location, 2))
            {
                if (m != this && m is Mobile m2 && CanBeHarmful(m2, false))
                {
                    DoHarmful(m2);
                    AOS.Damage(m2, this, Utility.RandomMinMax(25, 35), 0, 0, 0, 100, 0);
                    m2.Paralyze(TimeSpan.FromSeconds(2));
                }
            }
        }

        // Explosion of toxic gas on death
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*The covenant endures...*");
            PlaySound(0x208);
            for (int i = 0; i < 8; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);
                }
            }
        }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        public CultBrigand(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextVenomTime  = DateTime.UtcNow + TimeSpan.FromSeconds(12);
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextLeapTime   = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }
    }
}
