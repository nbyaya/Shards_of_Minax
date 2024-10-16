using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Network;

public class ChaosLord : BaseCreature
{
    private DateTime _nextConfusionTime;
    private DateTime _nextAoEDamageTime;
    private DateTime _nextImmunePhaseTime;
    private bool _inImmunePhase;

    public ChaosLord() : base(AIType.AI_Mage, FightMode.Weakest, 16, 1, 0.2, 0.4)
    {
        InitializeCreature();
    }

    public ChaosLord(Serial serial) : base(serial)
    {
    }

    private void InitializeCreature()
    {
        Name = "Chaos Lord";
        Body = 0x190;
        Hue = 0x482;
        BaseSoundID = 0x1F3;

        SetStr(500, 600);
        SetDex(200, 300);
        SetInt(300, 400);

        SetHits(1000, 1500);
        SetDamage(20, 30);

        SetResistance(ResistanceType.Physical, 60);
        SetResistance(ResistanceType.Fire, 60);
        SetResistance(ResistanceType.Cold, 60);
        SetResistance(ResistanceType.Poison, 60);
        SetResistance(ResistanceType.Energy, 60);

        SetSkill(SkillName.EvalInt, 120.0);
        SetSkill(SkillName.Magery, 120.0);
        SetSkill(SkillName.MagicResist, 120.0);
        SetSkill(SkillName.Tactics, 120.0);
        SetSkill(SkillName.Wrestling, 120.0);

        Fame = 20000;
        Karma = -20000;

        VirtualArmor = 60;

        _nextConfusionTime = DateTime.Now + TimeSpan.FromSeconds(30);
        _nextAoEDamageTime = DateTime.Now + TimeSpan.FromSeconds(10);
        _nextImmunePhaseTime = DateTime.Now + TimeSpan.FromSeconds(60);
    }

    public override void OnThink()
    {
        base.OnThink();

        if (DateTime.Now > _nextConfusionTime)
        {
            CastConfusion();
            _nextConfusionTime = DateTime.Now + TimeSpan.FromSeconds(30);
        }

        if (DateTime.Now > _nextAoEDamageTime)
        {
            CastAoEDamage();
            _nextAoEDamageTime = DateTime.Now + TimeSpan.FromSeconds(30);
        }

        if (DateTime.Now > _nextImmunePhaseTime)
        {
            ToggleImmunePhase();
            _nextImmunePhaseTime = DateTime.Now + TimeSpan.FromSeconds(60);
        }
    }

    private void CastConfusion()
    {
        // Add the actual implementation of the confusion spell here
    }

    private void CastAoEDamage()
    {
        foreach (Mobile m in GetMobilesInRange(5))
        {
            if (m is PlayerMobile)
            {
                m.SendMessage("You are engulfed in a magical storm!");
                AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 0, 0);
            }
        }
    }

    private void ToggleImmunePhase()
    {
        _inImmunePhase = !_inImmunePhase;

        if (_inImmunePhase)
        {
            this.SendMessage("Chaos Lord is now immune to damage!");
        }
        else
        {
            this.SendMessage("Chaos Lord's immunity has ended!");
        }
    }

    public override void OnDamage(int amount, Mobile from, bool willKill)
    {
        if (_inImmunePhase)
        {
            // During immune phase, do not take damage
            return;
        }
        base.OnDamage(amount, from, willKill);
    }

    public override void OnDeath(Container c)
    {
        base.OnDeath(c);

        c.DropItem(new Gold(1000, 1500));
        c.DropItem(new Longsword());
        c.DropItem(new Robe());
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version

        writer.Write(_inImmunePhase);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();

        _inImmunePhase = reader.ReadBool();

        // Re-initialize timers and other non-serialized fields
        _nextConfusionTime = DateTime.Now + TimeSpan.FromSeconds(30);
        _nextAoEDamageTime = DateTime.Now + TimeSpan.FromSeconds(10);
        _nextImmunePhaseTime = DateTime.Now + TimeSpan.FromSeconds(60);
    }

    public void ApplyBleedEffect(Mobile target)
    {
        Bleed b = new Bleed(target);
        b.Start();
    }

    private class Bleed
    {
        private Mobile _target;
        private Timer _timer;

        public Bleed(Mobile target)
        {
            _target = target;
            _timer = new BleedTimer(target);
        }

        public void Start()
        {
            _timer.Start();
        }

        private class BleedTimer : Timer
        {
            private Mobile _target;
            private int _ticks;

            public BleedTimer(Mobile target) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                _target = target;
                _ticks = 5; // Number of ticks (duration) of the bleed effect
            }

            protected override void OnTick()
            {
                if (_target == null || !_target.Alive || _ticks <= 0)
                {
                    Stop();
                    return;
                }

                _target.Damage(10, _target); // Bleed damage
                _target.SendMessage("You are bleeding!");
                _ticks--;
            }
        }
    }
}