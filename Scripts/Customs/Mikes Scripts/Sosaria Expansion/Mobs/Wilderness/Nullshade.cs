using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class NullifyingPresenceTimer : Timer
    {
        private readonly Nullshade _shade;

        public NullifyingPresenceTimer(Nullshade shade)
            : base(TimeSpan.FromSeconds(3.0), TimeSpan.FromSeconds(3.0))
        {
            _shade = shade;
            Priority = TimerPriority.FiftyMS;
        }

        protected override void OnTick()
        {
            if (_shade == null || _shade.Deleted)
            {
                Stop();
                return;
            }

            if (!_shade.Alive || _shade.Combatant == null)
                return;

            int drain = Utility.RandomMinMax(5, 10);
            foreach (Mobile m in _shade.GetMobilesInRange(6))
            {
                if (m != _shade && m.Player && _shade.CanBeHarmful(m))
                {
                    _shade.DoHarmful(m);

                    // Mana drain
                    if (m.Mana > 0)
                    {
                        int d = Math.Min(drain, m.Mana);
                        m.Mana -= d;
                    }

                    // Stam drain
                    if (m.Stam > 0)
                    {
                        int d = Math.Min(drain, m.Stam);
                        m.Stam -= d;
                    }
                }
            }
        }
    }

    [CorpseName("a nullshade corpse")]
    public class Nullshade : BaseCreature
    {
        private DateTime _nextPulse, _nextVoid, _nextStep;
        private NullifyingPresenceTimer _presenceTimer;

        [Constructable]
        public Nullshade()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a nullshade";
            Body = 26;
            Hue = 0x490;
            BaseSoundID = 0x482;

            SetStr(400, 500);
            SetDex(70, 90);
            SetInt(180, 250);

            SetHits(500, 600);
            SetMana(ManaMax);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 45, 55);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 45, 55);

            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 35;

            PackReg(20);
            // PackSlayer(); // <-- removed: not defined in ServUO
            PackNecroReg(15);

            _nextPulse = DateTime.UtcNow;
            _nextVoid  = DateTime.UtcNow;
            _nextStep  = DateTime.UtcNow;

            _presenceTimer = new NullifyingPresenceTimer(this);
            _presenceTimer.Start();
        }

        public Nullshade(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override TribeType Tribe => TribeType.Undead;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 4;

        public override void OnThink()
        {
            base.OnThink();
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat(); // make sure base runs if no specials

            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant))
                return;

            // 1) Nullifying Pulse
            if (DateTime.UtcNow >= _nextPulse && InRange(combatant, 8))
            {
                PerformNullifyingPulse();
                _nextPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                return;
            }

            // 2) Void Bolt
            if (DateTime.UtcNow >= _nextVoid && InRange(combatant, 10) && InLOS(combatant))
            {
                PerformVoidBolt(combatant);
                _nextVoid = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(4, 7));
            }

            // 3) Shadow Step
            var target = combatant;
            bool doStep = ((!InRange(target, 5) && Utility.RandomDouble() < 0.3)
                           || ((double)Hits / HitsMax < 0.3 && Utility.RandomDouble() < 0.5));

            if (doStep && DateTime.UtcNow >= _nextStep)
            {
                PerformShadowStep(target);
                _nextStep = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                return;
            }
        }

        public void PerformNullifyingPulse()
        {
            Animate(20, 7, 1, true, false, 0);
            PlaySound(0x165);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*begins to draw energy...*");

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                PlaySound(0x20F);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*unleashes a nullifying pulse!*");

                int dmg        = Utility.RandomMinMax(20, 35);
                int drain      = Utility.RandomMinMax(20, 40);
                TimeSpan dur   = TimeSpan.FromSeconds(8.0);

                foreach (Mobile m in GetMobilesInRange(6))
                {
                    if (m == this || !m.Player || !CanBeHarmful(m)) 
                        continue;

                    DoHarmful(m);
                    AOS.Damage(m, this, dmg, 0, 0, 40, 0, 60);

                    // Drain
                    if (m.Mana > 0) { int d = Math.Min(drain, m.Mana); m.Mana -= d; m.SendAsciiMessage("The nullifying pulse drains your mana!"); }
                    if (m.Stam > 0) { int d = Math.Min(drain, m.Stam); m.Stam -= d; m.SendAsciiMessage("The nullifying pulse saps your stamina!"); }

                    // Cold debuff
                    var coldMod = new ResistanceMod(ResistanceType.Cold, -15);
                    m.AddResistanceMod(coldMod);
                    m.SendAsciiMessage("Your resistance to cold is lowered!");

                    // Energy debuff
                    var enMod   = new ResistanceMod(ResistanceType.Energy, -15);
                    m.AddResistanceMod(enMod);
                    m.SendAsciiMessage("Your resistance to energy is lowered!");

                    // Schedule removal
                    Timer.DelayCall(dur, () =>
                    {
                        m.RemoveResistanceMod(coldMod);
                        m.SendAsciiMessage("Your resistance to cold returns to normal.");

                        m.RemoveResistanceMod(enMod);
                        m.SendAsciiMessage("Your resistance to energy returns to normal.");
                    });

                    // VFX
                    m.FixedParticles(0x3779, 1, 20, 9964, 1109, 0, EffectLayer.Waist);
                    m.PlaySound(0x233);
                }
            });
        }

        public void PerformVoidBolt(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive) return;

            Animate(9, 6, 1, true, false, 0);
            PlaySound(0x1E0);
            DoHarmful(target);

            MovingParticles(target, 0x379E, 7, 0, false, true, 0x490, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
            {
                if (target == null || target.Deleted || !target.Alive || target.Map != Map || !InRange(target, 10) || !InLOS(target))
                    return;

                target.PlaySound(0x208);

                int dmg   = Utility.RandomMinMax(25, 40);
                int drain = Utility.RandomMinMax(15, 30);
                AOS.Damage(target, this, dmg, 0, 0, 50, 0, 50);

                if (target.Mana > 0) { int d = Math.Min(drain, target.Mana); target.Mana -= d; target.SendAsciiMessage("A void bolt drains your mana!"); }
                if (target.Stam > 0) { int d = Math.Min(drain, target.Stam); target.Stam -= d; target.SendAsciiMessage("A void bolt saps your stamina!"); }

                target.FixedParticles(0x374A, 10, 15, 5013, 1109, 0, EffectLayer.Waist);
            });
        }

        public void PerformShadowStep(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive) return;

            Point3D p = Point3D.Zero;
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-5, 5);
                int y = Y + Utility.RandomMinMax(-5, 5);
                int z = Map.GetAverageZ(x, y);

                var loc = new Point3D(x, y, z);
                // 16 = approximate height, true = require surface, false = don't check stair tops
                if (Map.CanFit(loc, 16, true, false))
                {
                    p = loc;
                    break;
                }
            }

            if (p == Point3D.Zero || p == Location) return;

            FixedParticles(0x3728, 1, 13, 2499, 1109, 3, EffectLayer.Head);
            PlaySound(0x213);

            MoveToWorld(p, Map);

            FixedParticles(0x3728, 1, 13, 2499, 1109, 3, EffectLayer.Head);
            PlaySound(0x213);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Potions);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new NullshadeEssence());
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

            _presenceTimer = new NullifyingPresenceTimer(this);
            _presenceTimer.Start();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            _presenceTimer?.Stop();
            _presenceTimer = null;
        }
    }

    public class NullshadeEssence : Item
    {
        [Constructable]
        public NullshadeEssence() : base(0xF8F)
        {
            Name     = "Nullshade Essence";
            Hue      = 0x490;
            LootType = LootType.Blessed;
            Weight   = 0.1;
        }

        public NullshadeEssence(Serial serial) : base(serial) { }

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
